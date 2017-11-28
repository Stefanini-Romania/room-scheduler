import {Component,Output, ElementRef, AfterViewInit} from '@angular/core';
import {Router} from '@angular/router';
import {UserService} from '../../users/shared/users.service';
import { User } from '../../shared/models/user.model';

@Component({
    selector: 'admin-component',
    templateUrl: './admin.component.html',
    styleUrls: [],
    providers: [UserService],
})

export class AdminComponent implements AfterViewInit{

    public users:User[];
    public selectedUser: User;  

    constructor(private userService:UserService) {}

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
    }
}