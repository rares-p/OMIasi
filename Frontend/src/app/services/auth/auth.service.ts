import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { RegisterModel } from '../../models/auth/registerModel';
import { BaseResponse } from '../../models/responses/baseResponse';
import { catchError, map } from 'rxjs/operators';
import { firstValueFrom, of } from 'rxjs';
import { LoginModel } from '../../models/auth/loginModel';
import { BaseServerResponse } from '../../models/responses/baseServerResponse';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private readonly headers: HttpHeaders = new HttpHeaders().set(
        'Content-Type',
        'application/json'
    );
    private readonly emailPattern: RegExp =
        /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    private apiUrl = `${environment.apiUrl}`;

    constructor(private http: HttpClient) {}

    async register(registerModel: RegisterModel): Promise<BaseResponse> {
        console.log(registerModel);
        if (!registerModel.username.trim()) {
            return {
                success: false,
                error: 'Username is required.',
            };
        }

        if (!registerModel.password.trim()) {
            return {
                success: false,
                error: 'Password is required.',
            };
        }

        if (!registerModel.firstname.trim()) {
            return {
                success: false,
                error: 'First name is required.',
            };
        }

        if (!registerModel.lastname.trim()) {
            return {
                success: false,
                error: 'Last name is required.',
            };
        }

        if (!registerModel.email.trim()) {
            return {
                success: false,
                error: 'Email is required.',
            };
        } else if (!this.emailPattern.test(registerModel.email)) {
            return {
                success: false,
                error: 'Invalid email format.',
            };
        }

        return await firstValueFrom(
            this.http
                .post<BaseResponse>(
                    `${this.apiUrl}/register`,
                    JSON.stringify(registerModel),
                    {
                        headers: this.headers,
                        observe: 'response',
                    }
                )
                .pipe(
                    map((response) => ({
                        success: true,
                        error: null,
                    })),
                    catchError(async (error) => ({
                        success: false,
                        error: error.error,
                    }))
                )
        );
    }

    async login(loginModel: LoginModel): Promise<BaseResponse> {
        return await firstValueFrom(
            this.http
                .post<BaseServerResponse>(
                    `${this.apiUrl}/login`,
                    JSON.stringify(loginModel),
                    {
                        headers: this.headers,
                        observe: 'response',
                    }
                )
                .pipe(
                    map((response) => {
                        console.log(response);
                        if (response.status === 200) {
                            if (response.body?.value)
                                localStorage.setItem(
                                    'token',
                                    response.body.value
                                );
                            return { success: true, error: null };
                        } else {
                            const errorMessage = response.body!.error;
                            return { success: false, error: errorMessage };
                        }
                    }),
                    catchError(async (error) => {
                        console.error('Error during login:', error);
                        return {
                            success: false,
                            error: 'An error occurred during login. Please try again later.',
                        };
                    })
                )
        );
    }

    getUserToken() {
        return localStorage.getItem('token');
    }
}
