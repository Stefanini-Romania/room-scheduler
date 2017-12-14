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
    successfullAddRoom = new EventEmitter;
    successfullEditRoom = new EventEmitter;

    public model: Room = <Room> {};
    public title: string;
    public errorMessage: any ={};
    public selectedRoom: Room;
    rooms: Room[]= [];
    public errorMessages: any = {};

    constructor(private http: HttpClient, public activeModal: NgbActiveModal, private translate: TranslateService, private roomService: RoomService, private toastr: ToastrService, private modalService: NgbModal) {
    }
    
    ngOnInit() {
        this.title = this.model.id ? 'rooms.editRoom': 'rooms.add';
    }

    addRooms() {
        this.roomService.addRoom(this.model.name, this.model.location).subscribe(
            () => {
                this.successfullAddRoom.emit();
                this.toastr.success(
                    this.translate.instant('rooms.created'), '',
                    {positionClass: 'toast-bottom-right'}
                );                                                         
            },
            error => {
                this.errorMessages = {'generic': [error.error.message]};
    
                // build error message
                for (let e of error.error.errors) {
                    let field = 'generic';         
                    if (['Name', 'Location'].indexOf(e.field) >= 0) {
                        field = e.field;
                    }
                    if (!this.errorMessages[field]) {
                        this.errorMessages[field] = [];
                    }
                    this.errorMessages[field].push(e.errorCode);
                }
            });     
    }
    
    editRooms() {
        this.roomService.editRoom(this.model.id, this.model.name, this.model.location, this.model.isActive).subscribe(
            () => {
                this.successfullEditRoom.emit();
                this.toastr.success(
                    this.translate.instant('rooms.saved'), '',
                    {positionClass: 'toast-bottom-right'}
                )               
            },
            error => {
                this.errorMessages = {'generic': [error.error.message]};
                for (let e of error.error.errors) {
                    let field = 'generic';               
                    if (['Name', 'Location'].indexOf(e.field) >= 0) {
                        field = e.field;
                    }
                    if (!this.errorMessages[field]) {
                        this.errorMessages[field] = [];
                    }
                    this.errorMessages[field].push(e.errorCode);
                }
            });       
    } 

    inactiveRoom(Room) {   
        this.model.isActive = false;         
        this.roomService.editRoom(this.model.id, this.model.name, this.model.location, this.model.isActive).subscribe(
                () => {       
                    this.toastr.warning(
                        this.translate.instant('rooms.deleted'), '',
                        {positionClass: 'toast-bottom-right'}
                    );                
                   // this.refreshRooms();                      
                }, 
                error => {
                    this.errorMessage = error.error.message;
                });
    }
}