import {EventEmitter,Component, Output} from '@angular/core';
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
    @Output()
    successfullLogin = new EventEmitter;

    public errorMessage: string;

    model: User = <User> {};

    constructor(public activeModal: NgbActiveModal, private authService: AuthService, private router: Router) {
    }

    login() {
        this.authService.authenticate(this.model.name, this.model.password)
            .subscribe(
                () => {
                    this.successfullLogin.emit();
                    //this.activeModal.close();
                },
                error => {
                    this.errorMessage = error.error.message;
                });
    }

    register() {
        this.router.navigate(['/register']);
    }
}