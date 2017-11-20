import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {User} from '../../shared/models/user.model';
import {AuthService} from '../shared/auth.service';

@Component({
    selector: 'login-component',
    templateUrl: './login.component.html',
    styleUrls: [],
    providers: [AuthService],
})

export class LoginComponent {
    public errorMessage: string = '';

    model: User = <User> {};

    constructor(private authService: AuthService, private router: Router) {
    }

    ngAfterViewInit(): void {
        if(this.authService.isLoggedIn()) 
            this.router.navigate(['/calendar']);
    }

    login() {
        this.authService.authenticate(this.model.name, this.model.password)
            .subscribe(
                () => {
                    this.router.navigate(['/calendar']);

                },
                error => {
                    this.errorMessage = error.error.message;
                });
    }
}