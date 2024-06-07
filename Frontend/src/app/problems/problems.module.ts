import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AllProblemsComponent } from './all-problems/all-problems.component';
import { ProblemCardComponent } from './problem-card/problem-card.component';
import { ProblemComponent } from './problem/problem.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CreateProblemComponent } from './create-problem/create-problem.component';
import { EditProblemComponent } from './edit-problem/edit-problem.component';


@NgModule({
  declarations: [
    AllProblemsComponent,
    ProblemCardComponent,
    ProblemComponent,
    CreateProblemComponent,
    EditProblemComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ]
})
export class ProblemsModule { }