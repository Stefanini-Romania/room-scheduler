import {RoomSelector} from '../../rooms/room-selector/room-selector.component';
import {Component, EventEmitter, Output, ElementRef, AfterViewInit} from '@angular/core';
import {NgbModal, NgbModalRef, NgbPaginationConfig, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import {Router} from '@angular/router';
import {TranslateService} from "@ngx-translate/core";
import {ToastrService} from 'ngx-toastr';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

import {RoomService}  from './../../rooms/shared/room.service';
import {Room} from './../../shared/models/room.model';
import {UserService} from '../../users/shared/users.service';
import {User} from '../../shared/models/user.model';
import {RegisterFormComponent} from '../register-form/register-form.component';
import {RoomEditorComponent} from './../../rooms/room-editor/room-editor.component';
import {RoleEnum} from '../../shared/models/role.model';
import { AuthService } from '../../auth/shared/auth.service';

@Component({
    selector: 'admin-component',
    templateUrl: './admin.component.html',
    styleUrls: [],
    providers: [UserService, RoomService, NgbActiveModal],
})


export class AdminComponent implements AfterViewInit{

    @Output() 
    pageChange= new EventEmitter <number>();
    successfullInactiveUser = new EventEmitter;

    closeResult: string;
    public users: User[];
    public selectedUser: User;
    public rooms: Room[];
    public selectedRoom: Room;
    public user: User;
    public errorMessage: string;
    public model: User;
    currentUser: User;

    constructor(public activeModal: NgbActiveModal, private userService: UserService, 
        private roomService: RoomService, private modalService: NgbModal, private toastr: ToastrService, 
        private translate: TranslateService, private router: Router, private authService: AuthService
    ) {
        
    }
 
    ngAfterViewInit(): void {
        if (!this.authService.isLoggedIn() || !this.authService.getLoggedUser().hasRole(RoleEnum.admin)) {
            this.router.navigate(['calendar'])
        }

        this.refreshUsers();
        this.refreshRooms();
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

    refreshRooms() {
        this.rooms = [];
        this.roomService.roomList().subscribe((rooms: any) => {
            for (let room of rooms) {
                this.rooms.push(<Room>room);
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
        });
    }

    onActivateUser(model: User) {
        model.isActive = true;
        this.userService.editUser(model.id, model.firstName, model.lastName, model.name, model.email, model.departmentId, model.userRole, model.isActive, model.password)
        .subscribe(
            () => {   
                this.toastr.success(
                    this.translate.instant("user.active"), '',
                    {positionClass: 'toast-bottom-right'}
                );                
                this.refreshUsers();                      
            }, 
            error => {
                this.errorMessage = error.error.message;
            });
    }   
    
    onSelectRoom(model: Room) {
        const modalRef:NgbModalRef = this.modalService.open(RoomEditorComponent);   
        modalRef.componentInstance.model = model;  
        modalRef.componentInstance.successfullEditRoom.subscribe(() => {
            modalRef.close();     
        });       
    }

    onAddRoom() {
        const modalRef:NgbModalRef = this.modalService.open(RoomEditorComponent);
        modalRef.componentInstance.successfullAddRoom.subscribe(() => {
            modalRef.close();      
            this.refreshRooms();
        });       
    }
}
