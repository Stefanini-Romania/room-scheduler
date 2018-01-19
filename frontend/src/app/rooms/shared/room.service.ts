import {Injectable, EventEmitter} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/Rx';

import {environment} from '../../../environments/environment';
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
        return this.http.get(url, { headers: headers, withCredentials: true });
    }

    public selectRoom(room: Room) {
        this.selectedRoomChanged$.emit(room);
    }

    public addRoom(name: string, location: string) {
        const url = environment.apiUrl + '/room/add';
        const body = JSON.stringify({
            name: name,
            location: location        
        });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

        return this.http.post(url, body, {headers: headers, withCredentials: true})
            .catch((error: any) => {
                return Observable.throw(error);
            })
            .map((response: Response) => {
                return response;
            });
    }

    public editRoom(id: number, name: string, location: string, isActive?: boolean) {
        const url = environment.apiUrl + '/room/edit/' + id;
        const body = JSON.stringify({
            id: id,
            name: name,
            location: location,
            isActive: isActive        
        });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

        return this.http.put(url, body, {headers: headers, withCredentials: true})
            .catch((error: any) => {
                return Observable.throw(error);
            })
            .map((response: Response) => {
                return response;
            });
    }
} 
