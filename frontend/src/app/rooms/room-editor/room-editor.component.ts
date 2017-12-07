import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';
import {environment} from './../../../environments/environment';
import {RoomService} from './../shared/room.service';
import {Room} from './../../shared/models/room.model';

import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs/Observable';
import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {TranslateService} from "@ngx-translate/core";
import {ToastrService} from 'ngx-toastr';

@Component({
    
    selector: 'room-editor',
    templateUrl: './room-editor.component.html',
    styleUrls: [],
    providers: [RoomService, NgbActiveModal],
})

export class RoomEditorComponent {
    @Output()
    successfullAdd = new EventEmitter;

    public model: Room = <Room> {};
    public title: string;
    public errorMessages: any ={};
    public selectedRoom: Room;
    rooms: Room[]= [];

    constructor(private http: HttpClient, public activeModal: NgbActiveModal, private translate: TranslateService, private roomService: RoomService, private toastr: ToastrService) {
    }

    addRooms() {
        this.roomService.addRoom(this.model.name, this.model.location).subscribe(
            () => {
                this.successfullAdd.emit();
                this.toastr.success(
                    this.translate.instant('rooms.created'), '',
                    {positionClass: 'toast-bottom-right'}
                );                                                         
        });     
    }

    // deleteRooms() {
    //     this.roomService.deleteRoom(this.selectedRoom).subscribe(
    //         () => {
                
    //             this.toastr.success(
    //                 this.translate.instant('rooms.created'), '',
    //                 {positionClass: 'toast-bottom-right'}
    //             );                                                         
    //     });     
    // }

    editRooms() {
        //this.roomService.editRoom()
    }
}