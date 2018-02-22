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
    providers: [RoomService]
})

export class RoomEditorComponent {
    @Output()
    successfullAddRoom = new EventEmitter;
    successfullEditRoom = new EventEmitter;

    public model: Room = <Room> {};
    public modelForm: Room = <Room> {};
    public title: string;
    public errorMessage: any ={};
    public selectedRoom: Room;
    rooms: Room[]= [];
    public errorMessages: any = {};
    public isActive;

    constructor(private http: HttpClient, public activeModal: NgbActiveModal, private translate: TranslateService, private roomService: RoomService, private toastr: ToastrService, private modalService: NgbModal) {
    }
    
    ngOnInit() {
        this.title = this.model.id ? 'rooms.editRoom': 'rooms.add';
        this.modelForm.id = this.model.id;
        this.modelForm.name = this.model.name;
        this.modelForm.location = this.model.location;
        this.modelForm.isActive = this.model.isActive;
        if (this.modelForm.isActive == true) {
            this.isActive = 1;
        } 
        else this.isActive = 2; 
    }

    addRooms() {
        this.roomService.addRoom(this.modelForm.name, this.modelForm.location).subscribe(
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
        this.roomService.editRoom(this.modelForm.id, this.modelForm.name, this.modelForm.location, this.modelForm.isActive).subscribe(
            () => {
                this.model = this.modelForm;
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

    deactivateRoom(Room) {   
        this.modelForm.isActive = false;         
       
    }

    activateRoom(Room) {   
        this.modelForm.isActive = true;   
    }
}
