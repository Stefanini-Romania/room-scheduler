import {RoomSelector} from '../rooms/room-selector/room-selector.component';
import {Component, EventEmitter, Output, ElementRef, AfterViewInit} from '@angular/core';
import {NgbModal, NgbModalRef, NgbPaginationConfig, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import {Router} from '@angular/router';
import {TranslateService} from "@ngx-translate/core";
import {ToastrService} from 'ngx-toastr';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {RoomService}  from './../rooms/shared/room.service';
import {Room} from './../shared/models/room.model';
import {UserService} from '../users/shared/users.service';
import {User} from '../shared/models/user.model';
import {RegisterFormComponent} from '../users/register-form/register-form.component';
import {RoomEditorComponent} from './../rooms/room-editor/room-editor.component';
import {RoleEnum} from '../shared/models/role.model';
import { AuthService } from '../auth/shared/auth.service';
import{AdminUsersTab} from './admin-users-tab/admin-users-tab.component';
import{AdminRoomsTab} from './admin-rooms-tab/admin-rooms-tab.component';

@Component({
    selector: 'admin-component',
    templateUrl: './admin.component.html',
    styleUrls: [],
    providers: [UserService, RoomService, NgbActiveModal],
})


export class AdminComponent implements AfterViewInit{

    constructor(public activeModal: NgbActiveModal, private userService: UserService, 
        private roomService: RoomService, private modalService: NgbModal, private toastr: ToastrService, 
        private translate: TranslateService, private router: Router, private authService: AuthService
    ) {
        
    }
 
    ngAfterViewInit(): void {
        if (!this.authService.isLoggedIn() || !this.authService.getLoggedUser().hasRole(RoleEnum.admin)) {
            this.router.navigate(['calendar'])
        }


    }

}
