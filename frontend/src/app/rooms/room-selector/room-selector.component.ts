import {EventEmitter,Component, ViewChild, Output, ElementRef, AfterViewInit, OnDestroy} from '@angular/core';
import {TranslateService, LangChangeEvent} from "@ngx-translate/core";
import {NgbDropdownConfig} from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs/Subscription';

import {RoomService} from '../shared/room.service';
import {Room} from '../../shared/models/room.model';

@Component({
    selector: 'room-selector',
    templateUrl: './room-selector.component.html'
})
export class RoomSelector implements AfterViewInit, OnDestroy {
    @ViewChild('location') input: ElementRef;

    @Output()
    roomChange = new EventEmitter;

    public selectedRoomName: string;
    private subscription: Subscription;

    public selectedRoom: Room;

    public rooms: Room[] = [];
    public filteredRooms: Room[] = [];
    public location: string;

    constructor(config: NgbDropdownConfig, private roomService: RoomService, translate: TranslateService) {
        this.subscription = translate.onLangChange.subscribe((event: LangChangeEvent) => {
            this.selectedRoomName = translate.instant('calendar.rooms');
        });
    }

    ngAfterViewInit(): void {
        this.rooms = [];
        // @TODO Used only for tests TO BE REMOVED
        // let x=new Room();x.name="Room " + this.rooms.length + 1;x.location="Rome";this.rooms.push(x);
        // x=new Room();x.name="Room " + this.rooms.length + 1;x.location="Rome";this.rooms.push(x);
        // x=new Room();x.name="Room " + this.rooms.length + 1;x.location="Bucharest";this.rooms.push(x);
        // this.filteredRoomsByLocation();

        this.roomService.roomList().subscribe((rooms: any) => {
            for (let room of rooms) {
                this.rooms.push(<Room>room);
            }

            this.onSelectRoom(rooms[0]);
            this.filteredRoomsByLocation();
        });
    }


    focusOnLocation() {
        if (this.input) {
            setTimeout(()=> {
                this.input.nativeElement.focus();
            });
        }
    }

    ngOnDestroy() {
        // unsubscribe to ensure no memory leaks
        this.subscription.unsubscribe();
    }

    onSelectRoom(room: Room) {
        this.selectedRoom = room;

        // broadcast global event that room has changed
        this.roomService.selectRoom(room);

        this.roomChange.emit(room);
    }

    private assignCopy(){
        this.filteredRooms = Object.assign([], this.rooms);
        return this.filteredRooms;
    }

    filteredRoomsByLocation(location: string = '') {
        if(!location) {
            //when nothing has typed
            this.assignCopy();
        }

        this.filteredRooms = this.assignCopy().filter(
            (room:Room) => {
                return room.location.toLowerCase().indexOf(location.toLowerCase()) > -1
            }
        );
    }
}
