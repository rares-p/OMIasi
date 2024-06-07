import { Component, Input } from '@angular/core';
import { Problem } from '../../models/problems/problem';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
    selector: 'problem-card',
    templateUrl: './problem-card.component.html',
    styleUrls: ['./problem-card.component.css'],
})
export class ProblemCardComponent {
    @Input() problem: Problem | undefined;

    constructor(private router: Router, private authService: AuthService) {}

    solve(): void {
        console.log(this.problem);
        this.router.navigate([`problems/${this.problem!.title}`], {
            state: { problem: this.problem },
        });
    }

    edit() {
        this.router.navigate([`editProblem/${this.problem!.id}`], {
            state: { problem: this.problem },
        });
    }

    get isAdmin(): boolean {
        return this.authService.isAdmin();
    }
}
