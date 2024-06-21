import { Component, OnInit } from '@angular/core';
import { Problem } from '../../models/problems/problem';
import { ActivatedRoute, Router } from '@angular/router';
import * as CodeMirror from 'codemirror';
import { ProblemService } from '../../services/problems/problem.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../services/auth/auth.service';
import { SubmissionsService } from '../../services/submissions/submissions.service';
import { SubmissionModel } from '../../models/submissions/submissionModel';

@Component({
    selector: 'app-problem',
    templateUrl: './problem.component.html',
    styleUrls: ['./problem.component.css'],
})
export class ProblemComponent implements OnInit {
    problem: Problem | null | undefined;
    submissions: SubmissionModel[] | null | undefined;
    solution: string = `#include <iostream>

using namespace std;

int main()
{
    return 0;
}`;
    isLoggedIn: boolean = false;
    displayAllSubmissions: boolean = false;

    constructor(
        private problemService: ProblemService,
        private submissionService: SubmissionsService,
        private route: ActivatedRoute,
        private router: Router,
        private toasr: ToastrService,
        private authService: AuthService
    ) {}

    async ngOnInit(): Promise<void> {
        this.authService.isUserLoggedIn().subscribe((status) => {
            this.isLoggedIn = status;
        });

        const navigation = this.router.getCurrentNavigation();

        if (navigation?.extras.state && navigation.extras.state['problem']) {
            this.problem = navigation.extras.state['problem'] as Problem;
        } else {
            const problemId = this.route.snapshot.paramMap.get('id') ?? '';
            if (problemId) {
                this.problem = await this.problemService.getProblemById(
                    problemId
                );
                let submissionsResponse =
                    await this.submissionService.getAllSubmissions(
                        this.problem.id
                    );
                this.submissions = submissionsResponse.submissions;
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
        if (result.success) {
            this.toasr.success(
                `Problem ${this.problem.title} deleted sucessfully!`
            );
            this.router.navigate(['']);
        } else if (result.error) this.toasr.error(result.error);
        else this.toasr.error('Unknown error encountered from server.');
    }

    get isAdmin(): boolean {
        return this.authService.isAdmin();
    }

    async submitSolution() {
        this.toasr.info("Sending solution ... Please wait")
        let response = await this.submissionService.createSubmission({
            problemId: this.problem!.id,
            solution: this.solution,
        });
        if(!response.success)
            this.toasr.error(response.error ?? "Unknown error encountered, please try again later")
        let submissionsResponse =
            await this.submissionService.getAllSubmissions(this.problem!.id);
        this.submissions = submissionsResponse.submissions;
    }

    edit() {
        this.router.navigate([`editProblem/${this.problem!.id}`], {
            state: { problem: this.problem },
        });
    }

    viewAllSubmissions() {
        this.displayAllSubmissions = !this.displayAllSubmissions
    }

    displayedSubmissions(): SubmissionModel[] {
        if (!this.submissions) return [];
        return this.displayAllSubmissions
            ? this.submissions
            : this.submissions.slice(0, 3);
    }
}
