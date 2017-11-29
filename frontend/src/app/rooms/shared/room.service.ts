import {Injectable, EventEmitter} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { environment } from '../../../environments/environment';
import 'rxjs/Rx';

import {Room} from '../../shared/models/room.model';

@Injectable()
export class RoomService {
    public selectedRoomChanged$: EventEmitter<Room> = new EventEmitter();

    rooms: Room[];

    constructor(private http: HttpClient) {
    }

    public roomList() {
        const url = environment.apiUrl + '/room/list';
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url, {headers: headers});
    }

    public selectRoom(room: Room) {
        this.selectedRoomChanged$.emit(room);
    }
} 