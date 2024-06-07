import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { RegisterModel } from '../../models/auth/registerModel';
import { BaseResponse } from '../../models/responses/baseResponse';
import { catchError, map } from 'rxjs/operators';
import { BehaviorSubject, Observable, firstValueFrom, of } from 'rxjs';
import { LoginModel } from '../../models/auth/loginModel';
import { BaseServerResponse } from '../../models/responses/baseServerResponse';
import { JwtPayload, jwtDecode } from 'jwt-decode';
import { JwtInterface } from './jwt-interface';

@Injectable({
    providedIn: 'root',
})
export class AuthService {
    private loggedIn = new BehaviorSubject<boolean>(false);
    private readonly headers: HttpHeaders = new HttpHeaders().set(
        'Content-Type',
        'application/json'
    );
    private readonly emailPattern: RegExp =
        /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    private apiUrl = `${environment.apiUrl}`;

    constructor(private http: HttpClient) {
        const token = localStorage.getItem('token');
        this.loggedIn.next(!!token);
    }

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
                        if (!response.body)
                            return {
                                success: false,
                                error: "Can't communicate with server.",
                            };
                        if (response.body.isSuccess) {
                            if (response.body.value)
                                localStorage.setItem(
                                    'token',
                                    response.body.value
                                );
                            let role = jwtDecode<JwtInterface>(
                                response.body.value
                            ).role;
                            if (role) localStorage.setItem('role', role);
                            this.loggedIn.next(true);
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

    logout(): void {
        localStorage.removeItem('token');
        localStorage.removeItem('role');
        this.loggedIn.next(false);
    }

    getUserToken() {
        return localStorage.getItem('token');
    }

    getUserRole() {
        return localStorage.getItem('role');
    }

    isAdmin(): boolean {
        return this.getUserRole() == 'Admin' || this.getUserRole() == 'admin';
    }

    isUserLoggedIn(): Observable<boolean> {
        return this.loggedIn.asObservable();
    }
}
