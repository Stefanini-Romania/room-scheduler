import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {User} from '../../shared/models/user.model';
import {AuthService} from '../shared/auth.service';

@Component({
    selector: 'register-form',
    templateUrl: './register-form.component.html',
    styleUrls: [],
    providers: [AuthService],
})

export class RegisterFormComponent {
    public errorMessage: string = '';

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
}