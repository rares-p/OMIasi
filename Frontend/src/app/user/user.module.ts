import { NgModule } from '@angular/core';
import { LoginComponent } from './login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RegisterComponent } from './register/register.component';

@NgModule({
    declarations: [LoginComponent, RegisterComponent],
    imports: [FormsModule, CommonModule, ReactiveFormsModule],
})
export class UserModule {}
