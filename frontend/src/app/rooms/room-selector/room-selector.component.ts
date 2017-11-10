import {EventEmitter,Component, Input, Output, AfterViewInit, OnDestroy} from '@angular/core';
import {TranslateService, LangChangeEvent} from "@ngx-translate/core";
import { Subscription } from 'rxjs/Subscription';

import {RoomService} from '../shared/room.service';
import {Room} from '../../shared/models/room.model';

@Component({
    selector: 'room-selector',
    templateUrl: './room-selector.component.html'
})
export class RoomSelector implements AfterViewInit, OnDestroy {
    selectedRoomName: string ;
    subscription: Subscription;

    @Input()
    rooms: Room[];

    @Output()
    roomsLoaded = new EventEmitter;

    @Output()
    roomChange = new EventEmitter;

    constructor(private roomService: RoomService, translate: TranslateService) {
        this.subscription = translate.onLangChange.subscribe((event: LangChangeEvent) => {
            this.selectedRoomName = translate.instant('calendar.rooms');
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

    ngOnDestroy() {
        // unsubscribe to ensure no memory leaks
        this.subscription.unsubscribe();
    }

    onSelectRoom(room: Room) {
        this.selectedRoomName = room.name;
        this.roomChange.emit(room);
    }
}
