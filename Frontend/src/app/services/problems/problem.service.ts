import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, firstValueFrom, map } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Problem } from '../../models/problems/problem';
import { CreateProblem } from '../../models/problems/createProblem';
import { BaseResponse } from '../../models/responses/baseResponse';
import { BaseServerResponse } from '../../models/responses/baseServerResponse';

@Injectable({
    providedIn: 'root',
})
export class ProblemService {
    private apiUrl = `${environment.apiUrl}/problems`; // Replace with your API endpoint

    constructor(private http: HttpClient) {}

    getAllProblems(): Observable<Problem[]> {
        return this.http.get<Problem[]>(this.apiUrl);
    }

    getProblemById(id: string): Observable<Problem> {
        return this.http.get<Problem>(`${this.apiUrl}/${id}`);
    }

    getProblemByTitle(title: string): Observable<Problem> {
        return this.http.get<Problem>(`${this.apiUrl}/${title}`);
    }

    async createProblem(createProblem: CreateProblem): Promise<BaseResponse> {
        console.log(`Posting problem at${this.apiUrl}`, createProblem);
        return await firstValueFrom(
            this.http
                .post(this.apiUrl, createProblem, {
                    observe: 'response',
                })
                .pipe(
                    map(() => ({
                        success: true,
                        error: null,
                    })),
                    catchError(async (error) => {
                        console.error('Error during problem creation:', error);
                        return {
                            success: false,
                            error: error,
                        };
                    })
                )
        );
    }

    async deleteProblemById(id: string): Promise<BaseResponse> {
        return await firstValueFrom(
            this.http
                .delete<BaseServerResponse>(`${this.apiUrl}/${id}`, {
                    observe: 'response',
                })
                .pipe(
                    map(() => ({
                        success: true,
                        error: null,
                    })),
                    catchError(async (error) => {
                        if (error.status == 403)
                            return {
                                success: false,
                                error: 'Only admins can delete problems',
                            };
                        console.error('Error during problem deletion:', error);
                        return {
                            success: false,
                            error: error,
                        };
                    })
                )
        );
    }
}
