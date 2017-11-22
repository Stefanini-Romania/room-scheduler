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

    languages = [
        {'name': 'English', 'code': 'en', 'icon': 'https://cdn2.iconfinder.com/data/icons/flags_gosquared/64/United-Kingdom_flat.png' },
        {'name': 'Română', 'code': 'ro', 'icon': 'https://cdn2.iconfinder.com/data/icons/flags_gosquared/64/Romania_flat.png'}
    ];

    constructor(private authService: AuthService, private router: Router) {
    }

    get isLoggedIn(): boolean {
        this.currentUser = this.authService.getLoggedUser();
        return this.currentUser && this.authService.isLoggedIn();
    }

    logout() {
        this.authService.logout();
        // this.router.navigate(['/login']);
        
        
    }

    redirectToLogin() {
        this.router.navigate(['/login']);
    }
}