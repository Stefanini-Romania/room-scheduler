import { Component } from '@angular/core';
import { Room } from './room.model';

@Component({
    selector: 'room-selector',
    templateUrl: 'roomSelector.component.html'
})
export class RoomSelector {
    public rooms: [Room] = [
        {RoomId: 1, Name: 'ROOM 1', Location: 'http://google.ro?1'},
        {RoomId: 2, Name: 'ROOM 2', Location: 'http://google.ro?2'},
        {RoomId: 3, Name: 'ROOM 3', Location: 'http://google.ro?3'}
    ];

}
