import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Event } from '../../shared/event.model';
import {Jsonp, URLSearchParams } from '@angular/http';
import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs/operator/map';

@Injectable()
export class EventService{
    events: Event[];

    constructor(private http: HttpClient) {}
    
    public listEvents(startDate: Date, endDate: Date, roomId: number, hostId?: number){
        const url = 'http://172.25.4.165:88/api/event/list';
        const body = JSON.stringify({ startDate: startDate, endDate: endDate, roomId: roomId, hostId: hostId });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, body, { headers: headers })
        .catch((error:any) => Observable.throw(error.message))

        .map((response: Response) => {
            
            let event = response.json;
            // if (user) {
            //     localStorage.setItem('currentUser', JSON.stringify(user));
            // }
            // return user; 
        })
        .subscribe((data) => {
                console.log(data); 
        });
    }
}