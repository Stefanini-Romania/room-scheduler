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

    public roomList() {
        const url = 'http://fctestweb1:88/room/list';
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url, { headers: headers });
    }
} 