import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { TestFull } from '../../models/problems/testFull';
import { TestContent } from '../../models/tests/testContent';
import { TestContentResponse } from '../../models/responses/testContentResponse';
// import { problemTestDto } from '../../models/problems/testFull';

@Injectable({
    providedIn: 'root',
})
export class TestService {
    private apiUrl = `${environment.apiUrl}/tests`;

    constructor(private http: HttpClient) {}

    // async getAllTestsByProblemId(problemId: string): Promise<problemTestDto[]> {
    //     return await firstValueFrom(
    //         this.http.get<problemTestDto[]>(`${this.apiUrl}/${problemId}`)
    //     );
    // }

    async getTestContentById(testId: string): Promise<TestContentResponse> {
        return await firstValueFrom(
            this.http.get<TestContentResponse>(`${this.apiUrl}/${testId}`)
        );
    }

    async createTest(content: TestContent) {
        return await firstValueFrom(
            this.http.post<any>(`${this.apiUrl}`, content)
        );
    }
}
