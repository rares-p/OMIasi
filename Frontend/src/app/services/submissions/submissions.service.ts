import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { CreateSubmission } from '../../models/submissions/CreateSubmission';
import { BaseResponse } from '../../models/responses/baseResponse';
import { catchError, firstValueFrom, map } from 'rxjs';
import { SubmissionTestResult } from '../../models/submissions/submissionTestResult';
import { SubmissionResponse } from '../../models/responses/submissionResponse';

@Injectable({
    providedIn: 'root',
})
export class SubmissionsService {
    private apiUrl = `${environment.apiUrl}/submissions`;

    constructor(private http: HttpClient) {}

    async getAllSubmissions(problemId: string): Promise<SubmissionResponse> {
        var response = await firstValueFrom(this.http.get<SubmissionResponse>(`${this.apiUrl}/${problemId}`))
        response.submissions.sort((a, b) => a.date > b.date ? -1 : 1)
        return response
    }

    async createSubmission(
        createSubmission: CreateSubmission
    ): Promise<BaseResponse> {
        return await firstValueFrom(
            this.http
                .post(this.apiUrl, createSubmission, {
                    observe: 'response',
                })
                .pipe(
                    map(() => ({
                        success: true,
                        error: null,
                    })),
                    catchError(async (error) => {
                        console.error('Error during submission:', error);
                        return {
                            success: false,
                            error: error,
                        };
                    })
                )
        );
    }
}
