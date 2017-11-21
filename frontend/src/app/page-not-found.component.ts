import {Component} from '@angular/core';
import {AuthService} from './auth/shared/auth.service';
import {User} from './shared/models/user.model';

@Component({
    templateUrl: './page-not-found.component.html'
})

export class PageNotFoundComponent {
    
    currentUser: User = undefined;
    constructor(private authService: AuthService,) {}

    get isLoggedIn():boolean {
        this.currentUser = this.authService.getLoggedUser();
        return this.currentUser && this.authService.isLoggedIn();
    }
}

