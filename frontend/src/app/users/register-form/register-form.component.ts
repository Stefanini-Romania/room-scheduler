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

      
    public title: string;    
    public confirmPassword;
    public submitted;
    public model: User = <User>{
        departmentId: DepartmentIdEnum.ADC,
        userRole: [RoleEnum.attendee]
    };
    public modelForm: User = <User> {
        departmentId: DepartmentIdEnum.ADC,
        userRole: [RoleEnum.attendee]
    }
    public selectedRole = RoleEnum;

    public errorMessages: any = {};
    currentUser: User = undefined;
    DepartmentIdEnum: DepartmentIdEnum[] = [];
    RoleIdEnums = RoleEnum;
    RoleEnum: typeof RoleEnum = RoleEnum;
    public selectedUser: User;

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
        //TODO: find a more optimal solution
        this.modelForm.id = this.model.id;
        this.modelForm.firstName = this.model.firstName;
        this.modelForm.lastName = this.model.lastName;
        this.modelForm.name = this.model.name;
        this.modelForm.email = this.model.email;
        this.modelForm.departmentId = this.model.departmentId;
       // this.modelForm.userRole = this.model.userRole; //Fix
        this.modelForm.isActive = this.model.isActive;
    }

    get isLoggedIn():boolean {
        this.currentUser = this.authService.getLoggedUser();
        return this.currentUser && this.authService.isLoggedIn();
    }

    roleKeys() : Array<string> {
        let keys = Object.keys(this.RoleIdEnums);
        return keys.slice(keys.length / 2);
    }
   
    onRoleChange (userRoleId) {
        let userRole=RoleEnum[userRoleId];
        let roles =[];
        roles = [userRole];
        this.modelForm.userRole = roles;
    }

    register() {
        this.userService.createUser(this.modelForm.firstName, this.modelForm.lastName, this.modelForm.name, this.modelForm.email, this.modelForm.password, this.modelForm.departmentId, this.modelForm.userRole)
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

    editUser() {  
        this.userService.editUser(this.modelForm.id, this.modelForm.firstName, this.modelForm.lastName, this.modelForm.name, this.modelForm.email, this.modelForm.departmentId, this.modelForm.userRole, this.modelForm.isActive, this.modelForm.password)
        .subscribe(
            () => {
                this.model = this.modelForm;   
                this.successfullEditUser.emit();
                this.toastr.success(
                    this.translate.instant('user.saved'), '',
                    {positionClass: 'toast-bottom-right'}
                );         
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
        this.modelForm.isActive = false;       
    } 
    
    activateUser(User) {
        this.modelForm.isActive = true;
    }
}
    

   
