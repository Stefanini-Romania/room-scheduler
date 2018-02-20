import {Component, EventEmitter, Output, AfterViewInit} from '@angular/core';
import {NgbModal, NgbModalRef, NgbPaginationConfig, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import {ToastrService} from 'ngx-toastr';

import {TranslateService} from "@ngx-translate/core";
import {User} from '../../shared/models/user.model';
import {UserService} from '../../users/shared/users.service';
import {RoleEnum} from '../../shared/models/role.model';
import {RegisterFormComponent} from '../../users/register-form/register-form.component';
import {AuthService} from '../../auth/shared/auth.service';

@Component({
    selector: 'admin-users-tab',
    templateUrl: './admin-users-tab.component.html',
    styleUrls: [],
    providers: [UserService],
})

export class AdminUsersTab implements AfterViewInit{

    @Output() 
    pageChange= new EventEmitter <number>();
    successfullInactiveUser = new EventEmitter;

    public users: User[];
    public selectedUser: User;
    public user: User;
    public errorMessage: string;
    public model: User;
    currentUser: User;
    public page;

    constructor(private userService: UserService, private modalService: NgbModal, private translate: TranslateService, private toastr: ToastrService, private authService: AuthService) {
        
    }

    ngAfterViewInit(): void {
        if(this.authService.getLoggedUser().hasRole(RoleEnum.admin)){
            this.refreshUsers();
        }
    }

    refreshUsers() {
        this.users = [];
        this.userService.listUsers().subscribe((users: any) => {
            for (let user of users) { 
                for (let userRole of user.userRole) {
                    var index = user.userRole.indexOf(userRole);
                    user.userRole[index] = RoleEnum[userRole];
                }
                this.users.push(<User>user);  
            } 
        });
    }

    onAddUser() {
        const modalRef: NgbModalRef = this.modalService.open(RegisterFormComponent);
        modalRef.componentInstance.successfullAddUser.subscribe(() => {
            modalRef.close();
            this.refreshUsers();
        });     
    }

    onSelectUser(model: User) {
        const modalRef:NgbModalRef = this.modalService.open(RegisterFormComponent);    
        modalRef.componentInstance.model = model; 
        modalRef.componentInstance.successfullEditUser.subscribe(() => {
            modalRef.close();     
            this.refreshUsers();
        });   
    }
}