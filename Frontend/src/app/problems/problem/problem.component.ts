import { Component, Input, OnInit } from '@angular/core';
import { Problem } from '../../models/problems/problem';
import { ProblemService } from '../../services/problem.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-problem',
  templateUrl: './problem.component.html',
  styleUrl: './problem.component.css',
})
export class ProblemComponent implements OnInit {
  @Input() problem: Problem | null | undefined;

  constructor(
    private problemService: ProblemService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    if (this.problem) return;
    let problemId = this.route.snapshot.paramMap.get('id') ?? '';
    this.problemService.getProblemById(problemId).subscribe((data: Problem) => {
      this.problem = data;
    });
  }
}
