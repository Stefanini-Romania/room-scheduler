import { EventEmitter } from '@angular/core';
import { RoomService } from './room.service';
import { Component, Input, Output } from '@angular/core';
import { Room } from './room.model';
import { EventService} from './../calendars/shared/event.service';

@Component({
    selector: 'room-selector',
    templateUrl: 'roomSelector.component.html'
})
export class RoomSelector {
    @Input()
    rooms: Room[];
    
    constructor(private roomService: RoomService) {
       // this.select = new EventEmitter();
    }

    ngAfterViewInit(): void {
        this.rooms = [];
        this.roomService.roomList().subscribe((rooms: any) => {
            for (let room of rooms) {
                this.rooms.push(<Room>room);
            };
        });
    }


    @Output()
    roomChange = new  EventEmitter;
    
    onSelectRoom(room) {
      this.roomChange.emit(room);
    }
}
