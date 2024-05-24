import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ProblemsModule } from './problems/problems.module';
import { AllProblemsComponent } from './problems/all-problems/all-problems.component';
import { ProblemComponent } from './problems/problem/problem.component';
import { CreateProblemComponent } from './problems/create-problem/create-problem.component';
import { LoginComponent } from './user/login/login.component';

export const routes: Routes = [
    { path: '', component: AllProblemsComponent },
    { path: 'problems/:id', component: ProblemComponent },
    { path: 'createProblem', component: CreateProblemComponent },
    { path: 'login', component: LoginComponent }
];
