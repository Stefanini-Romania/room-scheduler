import {Component} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {RouterModule, Routes, Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {TranslateService} from "@ngx-translate/core";
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';


import {environment} from './../../../environments/environment';
import {DepartmentIdEnum} from './../../shared/models/departmentIdEnum.model';
import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';
import {RoleEnum} from '../../shared/models/role.model';
import {UserService} from '../shared/users.service';

@Component({
    moduleId: module.id,
    selector: 'register-form',
    templateUrl: './register-form.component.html',
    styleUrls: [],
    providers: [UserService],
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
                private userService:  UserService, 
                public activeModal: NgbActiveModal,
                private toastr: ToastrService,
                private translate: TranslateService) {
    }

    register() {
        this.userService.createUser(this.model.firstName, 
                                        this.model.lastName, 
                                        this.model.name, 
                                        this.model.email,
                                        this.model.password, 
                                        this.model.departmentId, 
                                        this.model.userRoles[0])
            
                                            
        .subscribe(
            () => {
                this.toastr.success(
                    this.translate.instant('User.Name.Created'), '',
                    {positionClass: 'toast-bottom-right'}
                ); 
                this.router.navigate(['/login']);
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
    
}
    

   
