import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';
import {environment} from './../../../environments/environment';
import {RoomService} from './../shared/room.service';
import {Room} from './../../shared/models/room.model';

import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs/Observable';
import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {NgbActiveModal, NgbModalRef, NgbModal} from '@ng-bootstrap/ng-bootstrap';
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
    successfullEdit = new EventEmitter;

    public model: Room = <Room> {};
    public title: string;
    public errorMessage: any ={};
    public selectedRoom: Room;
    rooms: Room[]= [];

    constructor(private http: HttpClient, public activeModal: NgbActiveModal, private translate: TranslateService, private roomService: RoomService, private toastr: ToastrService, private modalService: NgbModal) {
    }
    
    ngOnInit() {
        this.title = this.model.id ? 'rooms.editRoom': 'rooms.add';
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
    
    editRooms() {
        this.roomService.editRoom(this.model.id, this.model.name, this.model.location).subscribe(
            () => {
                this.successfullEdit.emit();
                this.toastr.success(
                    this.translate.instant('rooms.edited'), '',
                    {positionClass: 'toast-bottom-right'}
                )               
            });       
    } 
}