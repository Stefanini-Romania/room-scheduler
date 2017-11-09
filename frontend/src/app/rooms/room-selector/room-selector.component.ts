import {EventEmitter} from '@angular/core';
import {RoomService} from '../shared/room.service';
import {Component, Input, Output} from '@angular/core';
import {Room} from '../../shared/models/room.model';

@Component({
    selector: 'room-selector',
    templateUrl: './room-selector.component.html'
})
export class RoomSelector {
    @Input()
    rooms: Room[];

    constructor(private roomService: RoomService) {
    }

    ngAfterViewInit(): void {
        this.rooms = [];
        this.roomService.roomList().subscribe((rooms: any) => {
            for (let room of rooms) {
                this.rooms.push(<Room>room);
            }
        });
    }


    @Output()
    roomChange = new EventEmitter;

    onSelectRoom(room) {
        this.roomChange.emit(room);
    }
}
