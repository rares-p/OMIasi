import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { UserService } from '../../services/user.service';
import { LoginModel } from '../../models/auth/loginModel';
import { BaseResponse } from '../../models/responses/baseResponse';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginModel: LoginModel = {
    username: '',
    password: ''
  };
  loginError: string | null = null;
  private subscription?: Subscription;

  constructor(private userService: UserService) { }

  onSubmit(): void {
    this.subscription = this.userService.login(this.loginModel)
      .subscribe({
        next: (response: BaseResponse) => {
          if (response.success) {
            console.log('Login successful');
            // Redirect to another page or display a success message
          } else {
            console.error('Login failed:', response.error);
            this.loginError = response.error;
          }
        },
        error: (error: any) => {
          console.error('An unexpected error occurred during login:', error);
          this.loginError = 'An unexpected error occurred. Please try again later.';
        }
      });
  }

  ngOnDestroy(): void {
    // Unsubscribe from the observable to avoid memory leaks
    this.subscription?.unsubscribe();
  }
}
