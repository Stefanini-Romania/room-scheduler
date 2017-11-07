import { Subscription } from 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { Event } from '../../shared/event.model';
import {Jsonp, URLSearchParams } from '@angular/http';
import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs/operator/map';

@Injectable()
export class EventService{
    events: Event[];

    constructor(private http: HttpClient) {}
    
    public listEvents(startDate: Date, endDate: Date, roomId?: number, hostId?: number) {
        const url = 'http://fctestweb1:88/event/list';
        let params =  new HttpParams();
        params=params.append("startDate", startDate.toUTCString());
        if(endDate){
        params=params.append("endDate", endDate.toUTCString());
        }
        if(roomId>0){
            params=params.append("roomId", roomId.toString());
        }
        if(hostId>0){
            params=params.append("hostId", hostId.toString());
        }
        
        const body = JSON.stringify(params);
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url, { params: params });

    }

    public createEvents(startDate: Date, endDate: Date, eventType: number, roomId: number, hostId: number, attendeeId: number, eventStatus: number, notes?: string){
        const url = 'http://fctestweb1:88/event/create';
        const body = JSON.stringify({ startDate: startDate, endDate: endDate, eventType: eventType, roomId: roomId, hostId:hostId, attendeeId: attendeeId, eventStatus: eventStatus, notes:notes});
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, body, { headers: headers })
        
                        .catch((error:any) => Observable.throw(error.message))
                        .map((response: Response) => {
            
                                return response; 
                        })
        
        }
}
