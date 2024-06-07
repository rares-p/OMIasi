import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Problem } from '../../models/problems/problem';
import { ActivatedRoute, Router } from '@angular/router';
import * as CodeMirror from 'codemirror';
import { ProblemService } from '../../services/problems/problem.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth/auth.service';

@Component({
    selector: 'app-problem',
    templateUrl: './problem.component.html',
    styleUrls: ['./problem.component.css'],
})
export class ProblemComponent implements OnInit, AfterViewInit {
    problem: Problem | null | undefined;

    constructor(
        private problemService: ProblemService,
        private route: ActivatedRoute,
        private router: Router,
        private toasr: ToastrService,
        private authService: AuthService
    ) {}

    async ngOnInit(): Promise<void> {
        const navigation = this.router.getCurrentNavigation();
        console.log('Navigation state:', navigation?.extras.state);

        if (navigation?.extras.state && navigation.extras.state['problem']) {
            this.problem = navigation.extras.state['problem'] as Problem;
        } else {
            const problemId = this.route.snapshot.paramMap.get('id') ?? '';
            if (problemId) {
                this.problem = await this.problemService.getProblemById(problemId);
            }
        }
    }

    async deleteProblem(): Promise<void> {
        if (!this.problem) {
            this.toasr.error('Not a valid problem!');
            return;
        }
        let result = await this.problemService.deleteProblemById(
            this.problem.id
        );
        console.log("DELETION RESULT: ", result)
        if (result.success) {
            this.toasr.success(
                `Problem ${this.problem.title} deleted sucessfully!`
            );
            this.router.navigate([""]);
        } else if (result.error) this.toasr.error(result.error);
        else this.toasr.error('Unknown error encountered from server.');
    }

    get isAdmin(): boolean {
        return this.authService.isAdmin();
    }

    ngAfterViewInit(): void {
        // const editor = CodeMirror.fromTextArea(
        //   document.getElementById('code-editor') as HTMLTextAreaElement,
        //   {
        //     mode: 'text/x-c++src',
        //     lineNumbers: true,
        //     theme: 'dracula',
        //   }
        // );
    }
}
