import { Component } from '@angular/core';
import { User } from '../../shared/user.model';
import { AuthService } from '../../auth/shared/auth.service';
import { Router, RouterModule } from '@angular/router';

@Component ({
    selector: 'rs-header',
    templateUrl: './rs-header.component.html'
})

export class RSHeader{
    currentUser: User;
    constructor(private authService: AuthService, private router: Router) {
        this.currentUser = authService.getLoggedUser();
    }

    isLoggedIn(): boolean {
        return this.authService.isLoggedIn();
    }

    logout() {
        this.authService.logout();
        this.router.navigate(['/login']);
    }
}