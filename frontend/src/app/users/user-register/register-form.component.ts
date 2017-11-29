import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Observable} from 'rxjs/Observable';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';


import {environment} from './../../../environments/environment';
import {RoleEnum} from './../../shared/models/role.model';
import {DepartmentIdEnum} from './../../shared/models/departmentIdEnum.model';
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
    public model: User = {      
        firstName: '',
        lastName: '',
        name: '',
        email: '',
        password: '',  
        confirmPassword: '', 
        departmentId: 1,
        roleId: 1
    };
    
    public errorMessage: string = '';
    
    DepartmentIdEnum: DepartmentIdEnum[] = [];
    RoleEnum: RoleEnum[] = [];

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
                                        this.model.roleId)
        .subscribe(
            () => {
                this.activeModal.close();
               

            },
            error => {
                this.errorMessage = error.error.message;
            });
    }
}
    

   
