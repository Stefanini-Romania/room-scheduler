import { Subscription } from 'rxjs/Rx';
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
    
    public listEvents(startDate: Date, endDate: Date, roomId: number, hostId?: number) {
        const url = 'http://fctestweb1:88/event/list';
        const body = JSON.stringify({ startDate: startDate, endDate: endDate, roomId: roomId, hostId: hostId });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url, { headers: headers });
    }
}