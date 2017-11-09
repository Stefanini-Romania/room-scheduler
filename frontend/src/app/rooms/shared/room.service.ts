import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import 'rxjs/Rx';
import {Room} from '../../shared/models/room.model';

@Injectable()
export class RoomService {
    rooms: Room[];

    constructor(private http: HttpClient) {
    }

    public roomList() {
        const url = 'http://fctestweb1:88/room/list';
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url, {headers: headers});
    }
} 