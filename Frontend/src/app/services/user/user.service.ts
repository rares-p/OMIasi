import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { UserProfileResponse } from '../../models/responses/userProfileResponse';
import { ChangeRole } from '../../models/requests/changeRole';
import { ChangeRoleResponse } from '../../models/responses/changeRoleResponse';
import { BaseResponse } from '../../models/responses/baseResponse';

@Injectable({
    providedIn: 'root',
})
export class UserService {
    private apiUrl = `${environment.apiUrl}/users`;

    constructor(private http: HttpClient) {}

    async getProfile(username: string): Promise<UserProfileResponse> {
        return await firstValueFrom(
            this.http.get<UserProfileResponse>(
                `${this.apiUrl}/username?username=${username}`
            )
        );
    }

    async getUserByUsername(username: string): Promise<any> {
        return await firstValueFrom(
            this.http.get<any>(`${this.apiUrl}?username=${username}`)
        );
    }

    async updateUserRole(username: string, role: string): Promise<ChangeRoleResponse> {
        let newRole = role == "Teacher" ? 1 : 0
        let data: ChangeRole = {
            username: username,
            role: newRole
        }
        return await firstValueFrom(
            this.http.put<ChangeRoleResponse>(`${this.apiUrl}/changerole`, data)
        );
    }

    async deleteUser(username: string): Promise<BaseResponse> {
        return await firstValueFrom(this.http.delete<BaseResponse>(`${this.apiUrl}/username?username=${username}`))
    }
}
