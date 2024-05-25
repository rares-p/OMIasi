import { Component, OnInit } from '@angular/core';
import { Problem } from '../../models/problems/problem';
import { ProblemService } from '../../services/problems/problem.service';

@Component({
    selector: 'app-all-problems',
    templateUrl: './all-problems.component.html',
    styleUrls: ['./all-problems.component.css'],
})
export class AllProblemsComponent implements OnInit {
    problemsByYear: { [year: number]: Problem[] } = {};

    constructor(private problemService: ProblemService) {}

    ngOnInit(): void {
        this.problemService.getAllProblems().subscribe((data: Problem[]) => {
            this.groupProblemsByYear(data);
        });
    }

    private groupProblemsByYear(problems: Problem[]): void {
        problems.forEach((problem) => {
            const year = problem.year;
            if (!this.problemsByYear[year]) {
                this.problemsByYear[year] = [];
            }
            this.problemsByYear[year].push(problem);
        });
    }

    getYears(): number[] {
        return Object.keys(this.problemsByYear)
            .map((year) => parseInt(year))
            .sort((a, b) => b - a);
    }
}
