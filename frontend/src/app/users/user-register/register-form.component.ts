import {Router, RouterModule, Routes,} from '@angular/router';
import {Component} from '@angular/core';

import {UserService} from '../shared/users.service';
import {RoleEnum} from './../../shared/models/role.model';
import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';
import {RegisterService} from './../shared/register.service';

@Component({
    moduleId: module.id,
    selector: 'register-form',
    templateUrl: './register-form.component.html',
    styleUrls: [],
    providers: [AuthService, RegisterService, NgbActiveModal],
})

export class RegisterFormComponent {
    public confirmPassword;
 	submitted = false;
    public model: User = <User>{
        departmentId: DepartmentIdEnum.ADC,
        userRoles: [RoleEnum.attendee]
    };
    

    constructor(private authService: AuthService, 
                private router: Router, 
                private http: HttpClient, 
                private registerService:  RegisterService, 
                public activeModal: NgbActiveModal) {
    }

    
    
    register() {
        this.registerService.createUser(this.model.firstName, 
                                        this.model.lastName, 
                                        this.model.name, 
                                        this.model.email,
                                        this.model.password, 
                                        this.model.departmentId, 
                                        this.model.userRoles[1])
        .subscribe(
            () => {
                this.activeModal.close();
               

            },
            error => {
                this.errorMessage = error.error.message;
            });
    }
