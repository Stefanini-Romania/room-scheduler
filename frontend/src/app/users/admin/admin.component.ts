import {RoomSelector} from '../../rooms/room-selector/room-selector.component';
import {Component, EventEmitter, Output, ElementRef, AfterViewInit} from '@angular/core';
import {NgbModal, NgbModalRef, NgbPaginationConfig } from '@ng-bootstrap/ng-bootstrap';
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


@Component({
    selector: 'admin-component',
    templateUrl: './admin.component.html',
    styleUrls: [],
    providers: [UserService, RoomService, NgbActiveModal],
})

// @Input() id: string;
// @Input() maxSize: number;


export class AdminComponent implements AfterViewInit{

    @Output() 
    pageChange= new EventEmitter <number>();

    public users:User[];
    public selectedUser: User;
    public rooms: Room[];
    public selectedRoom: Room;
    public user: User;
    public errorMessage: string;

    public page : number;
    public total: number;
   
    constructor(public activeModal: NgbActiveModal,private userService:UserService, private roomService: RoomService, private modalService: NgbModal, private toastr: ToastrService, private translate: TranslateService, private router: Router) {
        
    }
 
    ngAfterViewInit(): void {
        this.refreshUsers();
        this.refreshRooms();
    }

    refreshUsers(){
        this.users= [];
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
        modalRef.result.then(() => {
            this.activeModal.close();
            this.refreshUsers();
        });
        
    }

    onSelectUser(user: User) {
        this.selectedUser = user;
        const modalRef:NgbModalRef = this.modalService.open(RegisterFormComponent);       
    }

    onSelectRoom(model: Room) {
        const modalRef:NgbModalRef = this.modalService.open(RoomEditorComponent);   
        modalRef.componentInstance.model = model;  
        modalRef.componentInstance.successfullEdit.subscribe(() => {
            modalRef.close();     
        });       
    }

    onAddRoom() {
        const modalRef:NgbModalRef = this.modalService.open(RoomEditorComponent);
        modalRef.componentInstance.successfullAdd.subscribe(() => {
            modalRef.close();      
            this.refreshRooms();
        });       
    }

    onDeleteRoom(room: Room) {
        //this.selectedRoom = room;            
        this.roomService.deleteRoom(room).subscribe(
                () => {       
                    this.toastr.warning(
                        this.translate.instant('room.deleted'), '',
                        {positionClass: 'toast-bottom-right'}
                    );                       
                    this.refreshRooms();                      
                }, 
                error => {
                    this.errorMessage = error.error.message;
                });
    }                                                         

    onDeleteUser(user) { //NOT FINISHED
        this.userService.deleteUser(user).subscribe(
                () => {                   
                        this.toastr.warning(
                            this.translate.instant("user.deleted"), '',
                            {positionClass: 'toast-bottom-right'}
                        );       
                        this.refreshUsers();                                                  
                },
                error => {
                    this.errorMessage = error.message;
                }); 
    }
}
