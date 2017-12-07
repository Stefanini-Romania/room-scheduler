import {Component, EventEmitter, Output, ElementRef, AfterViewInit} from '@angular/core';
import {NgbModal, NgbModalRef, NgbPaginationConfig } from '@ng-bootstrap/ng-bootstrap';
import {Router} from '@angular/router';
import {TranslateService} from "@ngx-translate/core";
import {ToastrService} from 'ngx-toastr';

import {RoomService}  from './../../rooms/shared/room.service';
import {Room} from './../../shared/models/room.model';
import {UserService} from '../../users/shared/users.service';
import {User} from '../../shared/models/user.model';
import {RegisterFormComponent} from '../register-form/register-form.component';
import {RoomEditorComponent} from './../../rooms/room-editor/room-editor.component';

@Component({
    selector: 'admin-component',
    templateUrl: './admin.component.html',
    styleUrls: [],
    providers: [UserService, RoomService],
})

// @Input() id: string;
// @Input() maxSize: number;


export class AdminComponent implements AfterViewInit{

    @Output() 
    pageChange= new EventEmitter <number>();

    public users:User[];
    public selectedUser: User;
    public rooms: Room[];
    public selectRoom: Room;
    public user: User;

    public page : number;
    public total: number;

    
         

    constructor(private userService:UserService, private roomService: RoomService, private modalService: NgbModal, private toastr: ToastrService, private translate: TranslateService) {
        
    }

    
    ngAfterViewInit(): void {
        
        this.users= [];
        this.rooms= [];
        
        this.userService.listUsers().subscribe((users: any) => {
            for (let user of users) {
                this.users.push(<User>user);
            }     
        });

        this.roomService.roomList().subscribe((rooms: any) => {
            for (let room of rooms) {
                this.rooms.push(<Room>room);
            }
        });
    }

    onAddUser() {
        const modalRef: NgbModalRef = this.modalService.open(RegisterFormComponent);
    }

    onSelectUser(user: User) {
        this.selectedUser = user;
        const modalRef:NgbModalRef = this.modalService.open(RegisterFormComponent);       
    }

    onSelectRoom(room :Room) {
        this.selectRoom = room;
        const modalRef:NgbModalRef = this.modalService.open(RoomEditorComponent);          
    }

    onAddRoom() {
        const modalRef:NgbModalRef = this.modalService.open(RoomEditorComponent);
        modalRef.componentInstance.successfullAdd.subscribe(() => {
            modalRef.close();
            this.roomService.roomList();
            
        });
    }

    // openRoomEdit(model: Room) {    
    //      const modalRef:NgbModalRef = this.modalService.open(RoomEditorComponent);  
    //      modalRef.componentInstance.model = model;  
    //      console.log("1");
    // }

    // showRoomEdit($event) {
    //     let model = this.rooms.find(e => e.id == $event.args.room.id);
    //     this.openRoomEdit(model);
    //     console.log("2");
    // }

    onDeleteRoom(room) { //NOT FINISHED
        //this.selectedRoom = room;            
        //this.roomService.selectRoom(rooms);
        //let model = this.rooms.find(e => e.id == $event.args.room.id);

        // console.log(room.id);
        console.log("HERE");
        this.roomService.deleteRoom(room).subscribe(
                    () => {                      
                        this.toastr.warning(
                            this.translate.instant('rooms.deleted'), '',
                            {positionClass: 'toast-bottom-right'}
                        );                                                         
                }); 
        //console.log(room);
        //this.model.id = null;
             
        // this.toastr.warning(
        //     this.translate.instant('rooms.deleted'), '',
        //     {positionClass: 'toast-bottom-right'}
        // );               
    }

    
    

    
}
