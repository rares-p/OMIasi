import { CommonModule, NgStyle } from '@angular/common';
import { AfterViewInit, ChangeDetectorRef, Component, Input } from '@angular/core';
import { SubmissionModel } from '../../models/submissions/submissionModel';

@Component({
    selector: 'app-submission-card',
    standalone: true,
    imports: [NgStyle, CommonModule],
    templateUrl: './submission-card.component.html',
    styleUrls: ['./submission-card.component.css'],
})
export class SubmissionCardComponent implements AfterViewInit {
    constructor(private cdr: ChangeDetectorRef) {}
    ngAfterViewInit(): void {
        this.score = this.getSubmssionScore(this.submission)
        this.cdr.detectChanges();
    }
    @Input() submission!: SubmissionModel;
    score: number = 0;
    isPopupVisible: boolean = false;

    getBackgroundColor(): string {
        if (this.score === 100) {
            return '#81C784';
        } else if (this.score === 0) {
            return '#E57373';
        } else if (this.score < 50) {
            return '#FFB74D';
        } else if (this.score < 90) {
            return '#FFF176';
        } else {
            return '#DCE775';
        }
    }

    openDetails(): void {
        this.isPopupVisible = true;
    }

    closeDetails(): void {
        this.isPopupVisible = false;
    }

    getSubmssionScore(submission: SubmissionModel): number {
        return submission.scores.reduce((sum, submission) => sum + submission.score, 0);
    }
}
