import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllProblemsComponent } from './all-problems/all-problems.component';
import { ProblemCardComponent } from './problem-card/problem-card.component';
import { ProblemComponent } from './problem/problem.component';
import { ReactiveFormsModule } from '@angular/forms';
import { CreateProblemComponent } from './create-problem/create-problem.component';


@NgModule({
  declarations: [
    AllProblemsComponent,
    ProblemCardComponent,
    ProblemComponent,
    CreateProblemComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule
  ]
})
export class ProblemsModule { }