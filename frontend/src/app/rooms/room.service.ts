import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Room } from './room.model';
import {Jsonp, URLSearchParams } from '@angular/http';
import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs/operator/map';

@Injectable()
export class RoomService {    
    rooms:Room[];
    
    constructor(private http: HttpClient) {}

    public roomList(RoomId: number, Name: string, Location: string) {
        const url = 'http://fctestweb1:88/room/list';
        const body = JSON.stringify({ RoomId: RoomId, Name: Name, Location: Location });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, body, { headers: headers })
        // .catch((error:any) => Observable.throw(error.message))
        
        // .map((response: Response) => {
            
        //     let room = response.json();
        //     // if (user) {
        //     //     localStorage.setItem('currentUser', JSON.stringify(user));
        //     // }
        //     // return user; 
        // })
        // .subscribe((data) => {
        //         console.log(data); 
        // });
    }
} 