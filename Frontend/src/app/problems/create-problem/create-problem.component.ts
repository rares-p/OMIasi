import { Component } from '@angular/core';
import { CreateProblem } from '../../models/problems/createProblem';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { problemTest } from '../../models/problems/problemTest';
import { ProblemService } from '../../services/problems/problem.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
    selector: 'app-create-problem',
    templateUrl: './create-problem.component.html',
    styleUrl: './create-problem.component.css',
})
export class CreateProblemComponent {
    problemForm: FormGroup;
    inputFiles: { file: File; index: number; score: number }[] = [];
    outputFiles: { file: File; index: number; score: number }[] = [];

    constructor(
        private fb: FormBuilder,
        private problemService: ProblemService,
        private toastr: ToastrService,
        private router: Router
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

    onFilesSelected(event: any, type: 'input' | 'output') {
        const files: File[] = event.target.files;
        const filesArray = Array.from(files).map((file, index) => ({
            file,
            index,
            score: 0,
        }));

        if (filesArray.length !== this.problemForm.value.noTests) {
            alert(
                `You must upload exactly ${this.problemForm.value.noTests} files for both input and output.`
            );
        } else {
            if (type === 'input') {
                this.inputFiles = filesArray;
            } else {
                this.outputFiles = filesArray;
            }
        }
    }

    adjustFileIndex(
        filesArray: { file: File; index: number }[],
        index: number,
        direction: 'up' | 'down'
    ) {
        const newIndex = direction === 'up' ? index - 1 : index + 1;
        if (newIndex < 0 || newIndex >= filesArray.length) {
            return;
        }
        const temp = filesArray[newIndex];
        filesArray[newIndex] = filesArray[index];
        filesArray[index] = temp;
    }

    async onSubmit(): Promise<void> {
        if (
            this.problemForm.valid &&
            this.inputFiles.length === this.problemForm.value.noTests &&
            this.outputFiles.length === this.problemForm.value.noTests
        ) {
            let tests: problemTest[] = [];
            for (let index = 0; index < this.inputFiles.length; index++) {
                tests.push({
                    index: index,
                    input: await this.readFileAsString(
                        this.inputFiles[index].file
                    ),
                    output: await this.readFileAsString(
                        this.outputFiles[index].file
                    ),
                    score: this.inputFiles[index].score,
                });
            }
            let createdProblem: CreateProblem = {
                title: this.problemForm.value.title,
                description: this.problemForm.value.description,
                noTests: this.problemForm.value.noTests,
                author: this.problemForm.value.author,
                timeLimitInSeconds: this.problemForm.value.timeLimitInSeconds,
                totalMemoryLimitInMb:
                    this.problemForm.value.totalMemoryLimitInMb,
                stackMemoryLimitInMb:
                    this.problemForm.value.stackMemoryLimitInMb,
                grade: this.problemForm.value.grade,
                inputFileName: this.problemForm.value.inputFileName,
                outputFileName: this.problemForm.value.outputFileName,
                year: this.problemForm.value.year,
                tests: tests,
            };

            let problemCreateResponse = await this.problemService.createProblem(
                createdProblem
            );
            console.log(problemCreateResponse)
            if (problemCreateResponse.success) {
                this.toastr.success('Problem created sucessfully');
                this.router.navigate(["problems", createdProblem.title]);
            } else
                this.toastr.error(
                    problemCreateResponse.error ??
                        'Unknown error encountered, please try again later.'
                );
        } else {
            console.log('Form is invalid or file count mismatch.');
        }
    }

    isFieldInvalid(field: string): boolean {
        const control = this.problemForm.get(field)!;
        return control.invalid;
    }

    isFileCountValid(): boolean {
        return (
            this.inputFiles.length === this.problemForm.value.noTests &&
            this.outputFiles.length === this.problemForm.value.noTests
        );
    }

    async readFileToBytesArray(file: File): Promise<string> {
        const arrayBuffer = await file.arrayBuffer();
        const int8Array = new Int8Array(arrayBuffer);

        const binaryString = Array.from(int8Array)
            .map((byte) => byte.toString(2).padStart(8, '0'))
            .join(' ');

        return binaryString;
    }

    async readFileAsString(file: File): Promise<string> {
        return await new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onload = () => {
                resolve(reader.result as string);
            };
            reader.onerror = reject;
            reader.readAsText(file);
        });
    }
}
