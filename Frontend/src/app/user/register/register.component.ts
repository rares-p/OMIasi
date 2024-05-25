import { Component } from '@angular/core';
import {
    FormBuilder,
    FormGroup,
    Validators,
    AbstractControl,
    ValidatorFn,
} from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css'],
})
export class RegisterComponent {
    registerForm: FormGroup;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private authService: AuthService,
        private toastr: ToastrService,
        private router: Router
    ) {
        this.registerForm = this.formBuilder.group(
            {
                firstName: ['', Validators.required],
                lastName: ['', Validators.required],
                email: ['', [Validators.required, Validators.email]],
                username: ['', Validators.required], // Added username field
                password: ['', [Validators.required, Validators.minLength(6)]],
                confirmPassword: ['', Validators.required],
            },
            {
                validators: this.mustMatch('password', 'confirmPassword'),
            }
        );
    }

    mustMatch(controlName: string, matchingControlName: string): ValidatorFn {
        return (formGroup: AbstractControl) => {
            const control = formGroup.get(controlName);
            const matchingControl = formGroup.get(matchingControlName);

            if (!control || !matchingControl) {
                return null;
            }

            if (matchingControl.errors && !matchingControl.errors.mustMatch) {
                return null;
            }

            if (control.value !== matchingControl.value) {
                matchingControl.setErrors({ mustMatch: true });
            } else {
                matchingControl.setErrors(null);
            }
            return null;
        };
    }

    get f() {
        return this.registerForm.controls;
    }

    async onSubmit() {
        this.submitted = true;

        if (this.registerForm.invalid) {
            return;
        }

        const response = await this.authService.register({
            username: this.registerForm.value.username,
            password: this.registerForm.value.password,
            firstname: this.registerForm.value.firstName,
            lastname: this.registerForm.value.lastName,
            email: this.registerForm.value.email,
        });

        if (response.error) this.toastr.error(response.error);
        else {
            this.toastr.success('User sucessfully registered!');
            this.router.navigate(['']);
        }
    }
}
