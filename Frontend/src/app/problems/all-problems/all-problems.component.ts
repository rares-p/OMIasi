import { Component } from '@angular/core';
import { Problem } from '../../models/problems/problem';
import { ProblemService } from '../../services/problem.service';

@Component({
  selector: 'app-all-problems-component',
  templateUrl: './all-problems.component.html',
  styleUrl: './all-problems.component.css'
})
export class AllProblemsComponent {

	problems: Problem[] = [];

	constructor(private problemService: ProblemService) {}

	ngOnInit(): void {
		this.problemService.getAllProblems().subscribe((data: Problem[]) => {
			this.problems = data;
		});
	}
}