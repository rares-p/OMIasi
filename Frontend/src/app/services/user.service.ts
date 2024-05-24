import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { RegisterModel } from '../models/auth/registerModel';
import { BaseResponse } from '../models/responses/baseResponse';
import { catchError, map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { LoginModel } from '../models/auth/loginModel';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    private readonly emailPattern: RegExp =
        /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
    private apiUrl = `${environment.apiUrl}`;

    constructor(private http: HttpClient) {}

    register(registerModel: RegisterModel): Observable<BaseResponse> {
        if (!registerModel.username.trim()) {
            return of({
                success: false,
                error: 'Username is required.',
            });
        }

        if (!registerModel.password.trim()) {
            return of({
                success: false,
                error: 'Password is required.',
            });
        }

        if (!registerModel.firstname.trim()) {
            return of({
                success: false,
                error: 'First name is required.',
            });
        }

        if (!registerModel.lastname.trim()) {
            return of({
                success: false,
                error: 'Last name is required.',
            });
        }

        if (!registerModel.email.trim()) {
            return of({
                success: false,
                error: 'Email is required.',
            });
        } else if (!this.emailPattern.test(registerModel.email)) {
            return of({
                success: false,
                error: 'Invalid email format.',
            });
        }

        return this.http
            .post(`${this.apiUrl}/register`, registerModel, {
                observe: 'response',
                responseType: 'text',
            })
            .pipe(
                map((response: HttpResponse<any>) => {
                    if (response.status === 201) {
                        return { success: true, error: null };
                    } else {
                        const errorMessage = response.body;
                        return { success: false, error: errorMessage };
                    }
                }),
                catchError((error) => {
                    console.error('Error registering user:', error);
                    return of({
                        success: false,
                        error: 'An error occurred while registering the user.',
                    });
                })
            );
    }

    login(loginModel: LoginModel): Observable<BaseResponse> {
        const headers = new HttpHeaders().set(
            'Content-Type',
            'application/json'
        );
        return this.http
            .post(`${this.apiUrl}/login`, JSON.stringify(loginModel), {
                headers: headers,
                observe: 'response',
                responseType: 'text',
            })
            .pipe(
                map((response) => {
                    if (response.status === 200) {
                        return { success: true, error: null };
                    } else {
                        const errorMessage = response.body;
                        return { success: false, error: errorMessage };
                    }
                }),
                catchError((error) => {
                    console.error('Error during login:', error);
                    return of({
                        success: false,
                        error: 'An error occurred during login. Please try again later.',
                    });
                })
            );
    }
}
