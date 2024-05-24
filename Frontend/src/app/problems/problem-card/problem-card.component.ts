import { Component, Input } from '@angular/core';
import { Problem } from '../../models/problems/problem';

@Component({
  selector: 'problem-card',
  templateUrl: './problem-card.component.html',
  styleUrls: ['./problem-card.component.css']
})

export class ProblemCardComponent {
  @Input() problem: Problem | undefined;
}
