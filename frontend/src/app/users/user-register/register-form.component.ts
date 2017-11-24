import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';


import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';

@Component({
    moduleId: module.id,
    selector: 'register-form',
    templateUrl: './register-form.component.html',
    styleUrls: [],
    providers: [AuthService],
})

export class RegisterFormComponent {
    public model: User = {
        name: '',
        password: '',
        email: '',
    };
    
    public errorMessage: string = '';

    constructor(private authService: AuthService, private router: Router) {
    }

    register() {
        /*this.userService.create(this.model)
            .subscribe(
                () => {
                    this.router.navigate(['/calendar']);
                },
                error => {
                    this.errorMessage = error.error.message;
                });*/
    }

   
}