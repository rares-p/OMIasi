import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ProblemService } from '../../services/problems/problem.service';
import {
    FormBuilder,
    FormGroup,
    Validators,
    FormsModule,
} from '@angular/forms';
import { ProblemFull } from '../../models/problems/problemFull';
import { TestFull } from '../../models/problems/testFull';
import { saveAs } from 'file-saver';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-edit-problem',
    templateUrl: './edit-problem.component.html',
    styleUrl: './edit-problem.component.css',
})
export class EditProblemComponent implements OnInit {
    problem!: ProblemFull;
    problemForm: FormGroup;

    constructor(
        private fb: FormBuilder,
        private router: Router,
        private route: ActivatedRoute,
        private problemService: ProblemService,
        private toastr: ToastrService
    ) {
        this.problemForm = this.fb.group({
            title: ['', [Validators.required, Validators.pattern(/^[A-Z].*$/)]],
            description: ['', Validators.required],
            year: [
                '',
                [
                    Validators.required,
                    Validators.min(1),
                    Validators.max(new Date().getFullYear()),
                ],
            ],
            noTests: [null, [Validators.required, Validators.min(1)]],
            author: ['', Validators.required],
            timeLimitInSeconds: [
                null,
                [Validators.required, Validators.min(0.0001)],
            ],
            totalMemoryLimitInMb: [null, Validators.min(0)],
            stackMemoryLimitInMb: [null, Validators.min(0)],
            grade: [
                null,
                [Validators.required, Validators.min(5), Validators.max(12)],
            ],
            inputFileName: [
                '',
                [
                    Validators.required,
                    Validators.pattern(/^[^\\\/:*?"<>|]+\.[a-zA-Z0-9]+$/),
                ],
            ],
            outputFileName: [
                '',
                [
                    Validators.required,
                    Validators.pattern(/^[^\\\/:*?"<>|]+\.[a-zA-Z0-9]+$/),
                ],
            ],
        });
    }

    async ngOnInit(): Promise<void> {
        const navigation = this.router.getCurrentNavigation();
        console.log('Navigation state:', navigation?.extras.state);

        const problemId = this.route.snapshot.paramMap.get('id') ?? '';
        if (problemId) {
            this.problem = await this.problemService.getFullProblemById(
                problemId
            );
        }

        if (this.problem) {
            await this.problem.tests.sort((a, b) => a.index - b.index);
            await this.problem.tests.forEach((test) => {
                test.input = this.binaryStringToByteArray(
                    test.input.toString()
                );
                test.output = this.binaryStringToByteArray(
                    test.output.toString()
                );
            });
            console.log(this.problem.tests);
            this.problemForm.patchValue(this.problem);
        }

        this.problemForm.valueChanges.subscribe((formValues) => {
            this.problem = { ...this.problem, ...formValues };
        });
    }

    binaryStringToByteArray(binaryString: string): Uint8Array {
        const numBytes = binaryString.length / 8;
        const byteArray = new Uint8Array(numBytes);

        for (let i = 0; i < numBytes; i++) {
            const byte = binaryString.substring(i * 8, i * 8 + 8);
            byteArray[i] = parseInt(byte, 2);
        }

        return byteArray;
    }

    isFieldInvalid(field: string): boolean {
        const control = this.problemForm.get(field)!;
        return control.invalid;
    }

    async updateProblem() {
        // this.problem = { ...this.problem, ...this.problemForm.value };
        let updateResult = await this.problemService.updateProblem(
            this.problem
        );
        if (updateResult.success) {
            this.toastr.success('Problem update sucessfully!');
            this.problem = await this.problemService.getFullProblemById(
                this.problem.id
            );
        } else
            this.toastr.error(
                updateResult.error ??
                    'Unknown error encoutered. Please try again later'
            );
    }

    downloadTestFile(test: TestFull, type: 'input' | 'output') {
        let data = new TextDecoder().decode(test[type]);
        let fileName =
            this.problem.inputFileName.split('.')[0] +
            '-' +
            test.index +
            (type == 'input' ? '.in' : 'out');
        saveAs(new Blob([data]), fileName);
    }

    async onFileSelected(
        event: any,
        testIndex: number,
        type: 'input' | 'output'
    ) {
        const file = event.target.files[0];
        if (file) {
            const fileContent = await this.readFileAsByteArray(file);
            if (type === 'input') {
                this.problem.tests[testIndex].input = fileContent;
            } else {
                this.problem.tests[testIndex].output = fileContent;
            }
        }
    }

    readFileAsByteArray(file: File): Promise<Uint8Array> {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = () => {
                const result = reader.result as ArrayBuffer;
                resolve(new Uint8Array(result));
            };
            reader.onerror = reject;
            reader.readAsArrayBuffer(file);
        });
    }

    onNoTestsChanged(): void {
        if (this.problem.noTests > this.problem.tests.length)
            this.problem.tests.push({
                id: null!,
                index: this.problem.noTests - 1,
                input: new Uint8Array(),
                output: new Uint8Array(),
                score: 0,
            });
        else this.problem.tests.pop();
        console.log(this.problem);
    }

    adjustFileIndex(index: number, direction: 'up' | 'down') {
        const newIndex = direction === 'up' ? index - 1 : index + 1;
        if (newIndex < 0 || newIndex >= this.problem.tests.length) {
            return;
        }
        this.problem.tests[index].index = newIndex;
        this.problem.tests[newIndex].index = index;
        const temp = this.problem.tests[newIndex];
        this.problem.tests[newIndex] = this.problem.tests[index];
        this.problem.tests[index] = temp;
        console.log(this.problem);
    }
}
