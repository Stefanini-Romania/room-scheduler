import {Component,Output, ElementRef, AfterViewInit} from '@angular/core';
import {NgbModal, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';
import {Router} from '@angular/router';

import {UserService} from '../../users/shared/users.service';
import { User } from '../../shared/models/user.model';
import {RegisterFormComponent} from '../user-register/register-form.component';

@Component({
    selector: 'admin-component',
    templateUrl: './admin.component.html',
    styleUrls: [],
    providers: [UserService],
})

export class AdminComponent implements AfterViewInit{

    public users:User[];
    public selectedUser: User;  

    constructor(private userService:UserService, private modalService: NgbModal) {}

    ngAfterViewInit(): void {
        
        this.users= [];
        
        this.userService.listUsers().subscribe((users: any) => {
            for (let user of users) {
                this.users.push(<User>user);
            }
        });
    }

    onSelectUser(user: User) {
        this.selectedUser = user;
        // const modalRef:NgbModalRef = this.modalService.open(RegisterFormComponent);
        
    }
}