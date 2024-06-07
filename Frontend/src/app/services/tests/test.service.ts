import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
// import { problemTestDto } from '../../models/problems/testFull';

@Injectable({
    providedIn: 'root',
})
export class TestService {
    private apiUrl = `${environment.apiUrl}/problems`;

    constructor(private http: HttpClient) {}

    // async getAllTestsByProblemId(problemId: string): Promise<problemTestDto[]> {
    //     return await firstValueFrom(
    //         this.http.get<problemTestDto[]>(`${this.apiUrl}/${problemId}`)
    //     );
    // }
}
