import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
    isLoggedIn: boolean = false;

    constructor(private authService: AuthService, private router: Router) {}

    ngOnInit(): void {
        this.authService.isUserLoggedIn().subscribe((status) => {
            this.isLoggedIn = status;
        });
    }

    logout(): void {
        this.authService.logout();
        console.log('LOGGED OUT');
    }

    async onCreateProblem(): Promise<void> {
        await this.router.navigate(['/createProblem']);
    }

    isAdmin(): boolean {
        return this.authService.isAdmin();
    }
}
