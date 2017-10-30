import { Component } from '@angular/core';
import { Room } from './room';

@Component({
    selector: 'room-selector',
    templateUrl: 'roomSelector.component.html'
})
export class Roomselector {
    private rooms: any = [
        {id: 1, name: 'ROOM 1', url: 'http://google.ro?1'},
        {id: 1, name: 'ROOM 2', url: 'http://google.ro?2'},
        {id: 1, name: 'ROOM 3', url: 'http://google.ro?3'}
    ];

}
