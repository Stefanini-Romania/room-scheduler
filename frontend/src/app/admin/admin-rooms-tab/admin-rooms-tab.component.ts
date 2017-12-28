import {Component, EventEmitter, Output, AfterViewInit} from '@angular/core';
import {NgbModal, NgbModalRef, NgbPaginationConfig, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import {ToastrService} from 'ngx-toastr';
import {TranslateService} from "@ngx-translate/core";

import {RoomService}  from '../../rooms/shared/room.service';
import {Room} from '../../shared/models/room.model';
import {RoomEditorComponent} from '../../rooms/room-editor/room-editor.component';

@Component({
    selector: 'admin-rooms-tab',
    templateUrl: './admin-rooms-tab.component.html',
    styleUrls: [],
    providers: [RoomService],
})

export class AdminRoomsTab implements AfterViewInit{
    @Output() 
    pageChange= new EventEmitter <number>();

    public rooms: Room[];
    public selectedRoom: Room;
    
    constructor(private roomService: RoomService, private modalService: NgbModal, private toastr: ToastrService, 
                private translate: TranslateService) {
        
    }

    ngAfterViewInit(){
        this.refreshRooms();
    }

    refreshRooms() {
        this.rooms = [];
        this.roomService.roomList().subscribe((rooms: any) => {
            for (let room of rooms) {
                this.rooms.push(<Room>room);
            }
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