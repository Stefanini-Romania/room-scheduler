import { RoomService } from './room.service';
import { Component } from '@angular/core';
import { Room } from './room.model';

@Component({
    selector: 'room-selector',
    templateUrl: 'roomSelector.component.html'
})
export class RoomSelector {
    rooms: Room[];

    constructor(private roomService: RoomService) {}

    ngAfterViewInit(): void {
        this.rooms = [];
        this.roomService.roomList().subscribe((rooms: any) => {
            for (let room of rooms) {
                this.rooms.push(<Room>room);
            };
        });
    }
}
