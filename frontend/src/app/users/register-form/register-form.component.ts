import {Component, Output, EventEmitter} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {RouterModule, Routes, Router} from '@angular/router';
import {ToastrService} from 'ngx-toastr';
import {TranslateService} from "@ngx-translate/core";
import {NgbActiveModal, NgbModal, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';

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
    providers: [UserService]
})

export class RegisterFormComponent {

    @Output()
    successfullAddUser = new EventEmitter;
    successfullEditUser = new EventEmitter;

    //refreshUsers  
    public title: string;    
    public confirmPassword;
    public submitted;
    public model: User = <User>{
        departmentId: DepartmentIdEnum.ADC,
        userRole: [RoleEnum.attendee]
    };
    
    public errorMessages: any = {};
    currentUser: User = undefined;
    DepartmentIdEnum: DepartmentIdEnum[] = [];
    RoleIdEnum: RoleEnum[] = [];

    constructor(private authService: AuthService, 
                private router: Router, 
                private http: HttpClient, 
                private userService:  UserService, 
                public activeModal: NgbActiveModal,
                private toastr: ToastrService,
                private translate: TranslateService,
                private modalService: NgbModal) {
    }

    ngOnInit() {
        this.title = this.model.id ? 'user.edit': 'user.add';
    }

    get isLoggedIn():boolean {
        this.currentUser = this.authService.getLoggedUser();
        return this.currentUser && this.authService.isLoggedIn();
    }

    register() {
        this.userService.createUser(this.model.firstName, 
                                        this.model.lastName, 
                                        this.model.name, 
                                        this.model.email,
                                        this.model.password, 
                                        this.model.departmentId,
                                        this.model.userRole
                                        )                                          
        .subscribe(
            () => {
                this.successfullAddUser.emit();
                this.toastr.success(
                    this.translate.instant('User.Name.Created'), '',
                    {positionClass: 'toast-bottom-right'}
                ); 
                if(!this.isLoggedIn){
                    this.router.navigate(['/login']);
                }

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

    editUser(){
        this.userService.editUser(this.model.id, this.model.firstName, this.model.lastName, this.model.name, this.model.email, this.model.departmentId, this.model.userRole, this.model.isActive, this.model.password).subscribe(
            () => {
                this.successfullEditUser.emit();
                this.toastr.success(
                    this.translate.instant('user.saved'), '',
                    {positionClass: 'toast-bottom-right'}
                )               
            },       
            error => {
                this.errorMessages = {'generic': [error.error.message]};
                for (let e of error.error.errors) {
                    let field = 'generic';
                    
                    if (['Name', 'Email'].indexOf(e.field) >= 0) {
                        field = e.field;
                    }

                    if (!this.errorMessages[field]) {
                        this.errorMessages[field] = [];
                    }
    
                    this.errorMessages[field].push(e.errorCode);
                }
            });       
    } 
    
    deactivateUser(User) {
        this.model.isActive = false;
        this.userService.editUser(this.model.id, this.model.firstName, this.model.lastName, this.model.name, this.model.email, this.model.departmentId, this.model.userRole, this.model.isActive, this.model.password)
        .subscribe(
            () => {   
                this.toastr.warning(
                    this.translate.instant("user.deactivated"), '',
                    {positionClass: 'toast-bottom-right'}
                );                
                //this.refreshUsers();                      
            }, 
            error => {
                this.errorMessages = error.error.message;
            });
    } 
    
    activateUser(User){
        this.model.isActive = true;
        this.userService.editUser(this.model.id, this.model.firstName, this.model.lastName, this.model.name, this.model.email, this.model.departmentId, this.model.userRole, this.model.isActive, this.model.password)
        .subscribe(
            () => {   
                this.toastr.success(
                    this.translate.instant("user.activated"), '',
                    {positionClass: 'toast-bottom-right'}
                );                                   
            }, 
            error => {
                this.errorMessages = error.error.message;
            });
    }
}
    

   
