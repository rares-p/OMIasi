import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Problem } from '../../models/problems/problem';
import { ActivatedRoute, Router } from '@angular/router';
import * as CodeMirror from 'codemirror';
import { ProblemService } from '../../services/problems/problem.service';

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
    private router: Router
  ) {}

  ngOnInit(): void {
    const navigation = this.router.getCurrentNavigation();
    console.log('Navigation state:', navigation?.extras.state);

    if (navigation?.extras.state && navigation.extras.state['problem']) {
      this.problem = navigation.extras.state['problem'] as Problem;
    } else {
      const problemId = this.route.snapshot.paramMap.get('id') ?? '';
      if (problemId) {
        this.problemService.getProblemById(problemId).subscribe((data: Problem) => {
          this.problem = data;
        });
      }
    }
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
