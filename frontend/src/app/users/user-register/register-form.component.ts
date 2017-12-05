import {Component} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {RouterModule, Routes, Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {TranslateService} from "@ngx-translate/core";

import {environment} from './../../../environments/environment';

import {DepartmentIdEnum} from './../../shared/models/departmentIdEnum.model';
import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';
import {RegisterService} from './../shared/register.service';
import {RoleEnum} from '../../shared/models/role.model';
import {UserService} from '../shared/users.service';

import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
    moduleId: module.id,
    selector: 'register-form',
    templateUrl: './register-form.component.html',
    styleUrls: [],
    providers: [AuthService, RegisterService],
})

export class RegisterFormComponent {
    public confirmPassword;
   userRoles

    public model: User = <User>{
        departmentId: DepartmentIdEnum.ADC,
        userRoles: [RoleEnum.attendee]
    };
    
    public errorMessages: any = {};
    
    DepartmentIdEnum: DepartmentIdEnum[] = [];
    RoleIdEnum: RoleEnum[] = [];

    constructor(private authService: AuthService, 
                private router: Router, 
                private http: HttpClient, 
                private registerService:  RegisterService, 
                public activeModal: NgbActiveModal,
                private toastr: ToastrService,
                private translate: TranslateService,) {
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

            },
            error => {
                this.errorMessages = {'generic': [error.error.message]};
    
                // build error message
                for (let e of error.error.errors) {
                    let field = 'generic';
                    
                    if (['FirstName', 'LastName', 'Name', 'Email', 'Password'].indexOf(e.field) >= 0) {
                        field = e.field;
                    }

                    if (!this.errorMessages[field]) {
                        this.errorMessages[field] = [];
                    }
    
                    this.errorMessages[field].push(e.errorCode);
                }
            });
        }

        // onSubmit(form) { 
        //     this.toastr.success(
        //         this.translate.instant('User.Name.Created'), '',
        //         {positionClass: 'toast-bottom-right'}
        //     ); 
        //     this.router.navigate(['/login']);
        // }
    
}
    

   
