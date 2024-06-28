import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
    selector: 'app-navbar',
    templateUrl: './navbar.component.html',
    styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent implements OnInit {
    isLoggedIn: boolean = false;
    searchQuery: string = '';

    constructor(private authService: AuthService, private router: Router, private toastr: ToastrService) {}

    ngOnInit(): void {
        this.authService.isUserLoggedIn().subscribe((status) => {
            this.isLoggedIn = status;
        });
    }

    logout(): void {
        this.authService.logout();
    }

    async onCreateProblem(): Promise<void> {
        await this.router.navigate(['/createProblem']);
    }

    isTeacher(): boolean {
        return this.authService.isTeacher();
    }

    async onSearch() {
        if(this.searchQuery == "")
            this.toastr.error("Cannot search with empty username")
        else
            await this.router.navigate([`/profile/${this.searchQuery}`]);
      }
}
