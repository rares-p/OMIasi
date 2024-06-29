import { Component, OnInit } from '@angular/core';
import { UserProfile } from '../models/user/userProfile';
import { UserService } from '../services/user/user.service';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Params, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth/auth.service';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-user-profile',
    standalone: true,
    imports: [CommonModule, RouterModule],
    templateUrl: './user-profile.component.html',
    styleUrl: './user-profile.component.css',
})
export class UserProfileComponent implements OnInit {
    user: UserProfile | undefined;
    username: string | undefined;

    constructor(
        private userService: UserService,
        private authService: AuthService,
        private route: ActivatedRoute,
        private toastr: ToastrService
    ) {}

    async ngOnInit(): Promise<void> {
        // this.username = this.route.snapshot.paramMap.get('username') ?? this.authService.getUserName()
        // if(this.username)
        //     this.user = await this.userService.getProfile(this.username);
        this.route.params.subscribe(async (params: Params) => {
            this.username =
                params['username'] || this.authService.getUserName();
            if (this.username) {
                let response = await this.userService.getProfile(this.username);
                if (response.success)
                    this.user = {
                        username: response.username,
                        role: response.role,
                        solvedProblems: response.solvedProblems,
                        attemptedProblems: response.attemptedProblems,
                    };
                else
                    this.toastr.error(
                        response.error ??
                            `Error searching user with username ${this.username}`
                    );
            }
        });

        console.log(this.authService.isAdmin());
    }

    get isAdmin(): boolean {
        return this.authService.isAdmin();
    }

    async updateUserRole(role: string): Promise<void> {
        if (!this.user) return;
        let response = await this.userService.updateUserRole(
            this.user.username,
            role
        );
        if (response.success) this.user.role = response.role;
    }

    async deleteUser(): Promise<void> {
        if (!this.user) return;
        let response = await this.userService.deleteUser(this.user.username);
        if (response.success) this.user = undefined;
    }
}
