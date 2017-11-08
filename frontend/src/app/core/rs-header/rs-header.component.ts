import {Component} from '@angular/core';
import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';
import {Router} from '@angular/router';

@Component({
    selector: 'rs-header',
    templateUrl: './rs-header.component.html'
})

export class RSHeader {
    currentUser: User = undefined;

    constructor(private authService: AuthService, private router: Router) {
    }

    isLoggedIn(): boolean {
        this.currentUser = this.authService.getLoggedUser();
        return this.currentUser && this.authService.isLoggedIn();
    }

    logout() {
        this.authService.logout();
        this.router.navigate(['/login']);
    }
}