import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, firstValueFrom, map } from 'rxjs';
import { environment } from '../../../environments/environment.development';
import { Problem } from '../../models/problems/problem';
import { CreateProblem } from '../../models/problems/createProblem';
import { BaseResponse } from '../../models/responses/baseResponse';
import { BaseServerResponse } from '../../models/responses/baseServerResponse';
import { ProblemFull } from '../../models/problems/problemFull';
import { ArrayUtilsService } from '../array-utils/array-utils.service';

@Injectable({
    providedIn: 'root',
})
export class ProblemService {
    private apiUrl = `${environment.apiUrl}/problems`; // Replace with your API endpoint

    constructor(private http: HttpClient, private utils: ArrayUtilsService) {}

    getAllProblems(): Observable<Problem[]> {
        return this.http.get<Problem[]>(this.apiUrl);
    }

    async getProblemById(id: string): Promise<Problem> {
        return await firstValueFrom(
            this.http.get<Problem>(`${this.apiUrl}/${id}`)
        );
    }

    async getFullProblemById(id: string): Promise<ProblemFull> {
        return await firstValueFrom(
            this.http.get<ProblemFull>(`${this.apiUrl}/admin/${id}`)
        );
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

    async updateProblem(problem: ProblemFull): Promise<Problem> {
        const problemToSend = {
            ...problem,
            tests: problem.tests.map((test) => ({
                ...test,
                input: this.utils.arrayBufferToBase64(test.input),
                output: this.utils.arrayBufferToBase64(test.output),
            })),
        };
        return await firstValueFrom(
            this.http.put<any>(`${this.apiUrl}`, problemToSend)
        );
    }

    async readFileToBytesArray(file: File): Promise<string> {
        const arrayBuffer = await file.arrayBuffer();
        const int8Array = new Int8Array(arrayBuffer);

        const binaryString = Array.from(int8Array)
            .map((byte) => byte.toString(2).padStart(8, '0'))
            .join(' ');

        return binaryString;
    }
}
