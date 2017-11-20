import {EventEmitter,Component, Output, AfterViewInit, OnDestroy} from '@angular/core';
import {TranslateService, LangChangeEvent} from "@ngx-translate/core";
import { Subscription } from 'rxjs/Subscription';

import {RoomService} from '../shared/room.service';
import {Room} from '../../shared/models/room.model';

@Component({
    selector: 'room-selector',
    templateUrl: './room-selector.component.html'
})
export class RoomSelector implements AfterViewInit, OnDestroy {
    @Output()
    roomChange = new EventEmitter;

    public selectedRoomName: string;
    private subscription: Subscription;

   public selectedRoom: Room;

    public rooms: Room[] = [];

    constructor(private roomService: RoomService, translate: TranslateService) {
        this.subscription = translate.onLangChange.subscribe((event: LangChangeEvent) => {
            this.selectedRoomName = translate.instant('calendar.rooms');
        });
    }

    ngAfterViewInit(): void {
        this.rooms = [];
        /*
        // @TODO Used only for tests TO BE REMOVED
        let x=new Room();x.name="Room " + this.rooms.length + 1;this.rooms.push(x);
        x=new Room();x.name="Room " + this.rooms.length + 1;this.rooms.push(x);
        x=new Room();x.name="Room " + this.rooms.length + 1;this.rooms.push(x);
        */

        this.roomService.roomList().subscribe((rooms: any) => {
            for (let room of rooms) {
                this.rooms.push(<Room>room);
            }

            this.onSelectRoom(rooms[0]);
        });
    }

    ngOnDestroy() {
        // unsubscribe to ensure no memory leaks
        this.subscription.unsubscribe();
    }

    onSelectRoom(room: Room) {
        this.selectedRoom = room;
        this.roomChange.emit(room);
    }
}
