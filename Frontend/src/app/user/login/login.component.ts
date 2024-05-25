import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../../services/auth/auth.service';
import { LoginModel } from '../../models/auth/loginModel';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
})
export class LoginComponent {
    loginModel: LoginModel = {
        username: '',
        password: '',
    };
    loginError: string | null = null;

    constructor(
        private authService: AuthService,
        private toastr: ToastrService,
        private router: Router
    ) {}

    async onSubmit(): Promise<void> {
        const response = await this.authService.login(this.loginModel);
        if (response.error) this.toastr.error(response.error);
        else {
            this.toastr.success('User sucessfully registered!');
            this.router.navigate(['']);
        }
    }
}
