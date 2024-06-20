import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { UserProfileResponse } from '../../models/responses/userProfileResponse';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    private apiUrl = `${environment.apiUrl}/users`;

    constructor(private http: HttpClient) {}

    async getProfile(username: string) : Promise<UserProfileResponse> {
        return await firstValueFrom(
            this.http.get<UserProfileResponse>(`${this.apiUrl}/username?username=${username}`)
        );
    }
}
