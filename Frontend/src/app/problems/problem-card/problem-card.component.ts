import { Component, Input } from '@angular/core';
import { Problem } from '../../models/problems/problem';
import { Router } from '@angular/router';

@Component({
    selector: 'problem-card',
    templateUrl: './problem-card.component.html',
    styleUrls: ['./problem-card.component.css'],
})
export class ProblemCardComponent {
    @Input() problem: Problem | undefined;

    constructor(private router: Router) {}

    solve(): void {
        console.log(this.problem)
        this.router.navigate([`problems/${this.problem!.title}`], {
            state: { problem: this.problem },
        });
    }
}
