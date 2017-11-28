import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';

@Component({
    selector: 'login-form',
    templateUrl: './login-form.component.html',
    styleUrls: [],
    providers: [NgbActiveModal, AuthService],
})

export class LoginFormComponent {
    public errorMessage: string;

    model: User = <User> {};

    constructor(private authService: AuthService, private router: Router) {
    }

    login() {
        this.authService.authenticate(this.model.name, this.model.password)
            .subscribe(
                () => {
                },
                error => {
                    this.errorMessage = error.error.message;
                });
    }

    register() {
        this.router.navigate(['/register']);
    }
}