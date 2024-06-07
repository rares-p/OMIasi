import { Routes } from '@angular/router';
import { AllProblemsComponent } from './problems/all-problems/all-problems.component';
import { ProblemComponent } from './problems/problem/problem.component';
import { CreateProblemComponent } from './problems/create-problem/create-problem.component';
import { LoginComponent } from './user/login/login.component';
import { RegisterComponent } from './user/register/register.component';
import { EditProblemComponent } from './problems/edit-problem/edit-problem.component';

export const routes: Routes = [
    { path: '', component: AllProblemsComponent },
    { path: 'problems/:id', component: ProblemComponent },
    { path: 'createProblem', component: CreateProblemComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'editProblem/:id', component: EditProblemComponent}
];
