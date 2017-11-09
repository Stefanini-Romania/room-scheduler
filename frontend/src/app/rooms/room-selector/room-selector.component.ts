import {EventEmitter,Component, Input, Output} from '@angular/core';
import {TranslateService} from "@ngx-translate/core";

import {RoomService} from '../shared/room.service';
import {Room} from '../../shared/models/room.model';

@Component({
    selector: 'room-selector',
    templateUrl: './room-selector.component.html'
})
export class RoomSelector {
    selectedRoomName: string ;

    @Input()
    rooms: Room[];

    @Output()
    roomsLoaded = new EventEmitter;

    @Output()
    roomChange = new EventEmitter;

    constructor(private roomService: RoomService, translate: TranslateService) {
        translate.get('calendar.rooms').subscribe((translation: string) => {
            this.selectedRoomName = translation;
        });
    }

    ngAfterViewInit(): void {
        this.rooms = [];
        this.roomService.roomList().subscribe((rooms: any) => {
            for (let room of rooms) {
                this.rooms.push(<Room>room);
            }

            this.roomsLoaded.emit(this.rooms);
        });
    }

    onSelectRoom(room: Room) {
        this.selectedRoomName = room.name;
        this.roomChange.emit(room);
    }
}
