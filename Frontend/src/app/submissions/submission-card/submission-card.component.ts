import { CommonModule, NgStyle } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
    selector: 'app-submission-card',
    standalone: true,
    imports: [NgStyle, CommonModule],
    templateUrl: './submission-card.component.html',
    styleUrl: './submission-card.component.css',
})
export class SubmissionCardComponent {
    @Input() score!: number;
    @Input() date!: Date;

    getBackgroundColor(): string {
        if (this.score === 100) {
            return 'green';
        } else if (this.score === 0) {
            return 'red';
        } else if (this.score < 50) {
            return 'yellow';
        } else {
            return 'lightgreen';
        }
    }
}
