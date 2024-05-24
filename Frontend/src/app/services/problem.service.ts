import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { Problem } from '../models/problems/problem';
import { CreateProblem } from '../models/problems/createProblem';

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

    createProblem(createProblem: CreateProblem) {
        console.log(`Posting problem at${this.apiUrl}`, createProblem);
        this.http
            .post<CreateProblem>(this.apiUrl, createProblem)
            .subscribe((val) => console.log('getting log value', val));
    }
}
