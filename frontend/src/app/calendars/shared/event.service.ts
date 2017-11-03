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
    
    public listEvents(startDate: Date, endDate: Date, roomId: number, hostId?: number) {
        const url = 'http://fctestweb1:88/event/list';
        let Params = new HttpParams();
        Params = Params.append('startDate', startDate.toUTCString());
        Params = Params.append('endDate', endDate.toUTCString());
        if (roomId>0){
            Params = Params.append('roomId', roomId.toString());    
        }
        
        if (hostId>0){
            Params = Params.append('hostId?', hostId.toString());
        }


        const body = JSON.stringify(Params);

        return this.http.get(url, { params: Params });
       
    }
}
