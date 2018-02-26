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
        userRole: [RoleEnum.attendee]
    };
    public modelForm: User = <User> {
        userRole: [RoleEnum.attendee]
    }
    public selectedRole = RoleEnum;
    public errorMessages: any = {};
    public selectedUser: User;
    public user;
    RoleEnum: typeof RoleEnum = RoleEnum;
    currentUser: User = undefined;
    DepartmentIdEnum: DepartmentIdEnum[] = [];
    RoleIdEnums = RoleEnum;
    displayRole;
    isActive;
    isDept;


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
        this.modelForm.id = this.model.id;
        this.modelForm.firstName = this.model.firstName;
        this.modelForm.lastName = this.model.lastName;
        this.modelForm.email = this.model.email;
        this.modelForm.departmentId = this.model.departmentId;
        //this.modelForm.userRole = this.model.userRole; //Fix
        this.modelForm.isActive = this.model.isActive;

        if (this.modelForm.isActive == true) {
            this.isActive = 1;
        } 
        else this.isActive = 2; 
           
        if (this.modelForm.id) {
            if (this.modelForm.departmentId == 1) {
                this.isDept = 1;
            } 
            else this.isDept = 2;        
        }    
       this.onRoleSelect(this.displayRole);
    }
 
    get isLoggedIn():boolean {
        this.currentUser = this.authService.getLoggedUser();
        return this.currentUser && this.authService.isLoggedIn();
    }

    isAdmin(currentUser: User): boolean {
        return (currentUser && currentUser.userRole.length != 0 && currentUser.userRole.indexOf(RoleEnum.admin) !== -1);
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

    onRoleSelect (selectedRole: string) {
        if (this.model.id) {
            this.displayRole = this.model.userRole[0];         
            switch(selectedRole) {
                case "attendee":                     
                case "host":                        
                case "admin":
                    this.displayRole = selectedRole;
                    break;
                default:
                    //this.displayRole = selectedRole;
            } 
        } 
        else {     
            switch(selectedRole) {
                case "attendee":
                case "host":          
                case "admin":
                    this.displayRole = selectedRole;
                    break;
                default:
                    this.displayRole = this.RoleEnum[1];
            }        
        }    
    }

    register() {
        this.userService.registerUser(this.modelForm.firstName, this.modelForm.lastName, this.modelForm.email, this.modelForm.password, this.modelForm.departmentId)
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
                    
                    if (['FirstName', 'LastName', 'Email', 'Password'].indexOf(e.field) >= 0) {
                        field = e.field;
                    }

                    if (!this.errorMessages[field]) {
                        this.errorMessages[field] = [];
                    }
    
                    this.errorMessages[field].push(e.errorCode);
                }
            });
    }
    addByAdmin() {
        this.userService.addUser(this.modelForm.firstName, this.modelForm.lastName, this.modelForm.email, this.modelForm.password, this.modelForm.departmentId, this.modelForm.userRole)
        .subscribe(
            () => {
                this.successfullAddUser.emit();
                this.toastr.success(
                    this.translate.instant('User.Name.Created'), '',
                    {positionClass: 'toast-bottom-right'}
                );
            },
            error => {
                this.errorMessages = {'generic': [error.error.message]};
                // build error message
                for (let e of error.error.errors) {
                    let field = 'generic';
                    
                    if (['FirstName', 'LastName', 'Email', 'Password'].indexOf(e.field) >= 0) {
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
        this.userService.editUser(this.modelForm.id, this.modelForm.firstName, this.modelForm.lastName, this.modelForm.email, this.modelForm.departmentId, this.modelForm.userRole, this.modelForm.isActive, this.modelForm.password)
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
                    if (['Email'].indexOf(e.field) >= 0) {
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

    adcDepartment(User){
        this.modelForm.departmentId = 1;
    }

    sdcDepartment(User){
        this.modelForm.departmentId = 2;
    }
}
    

   
