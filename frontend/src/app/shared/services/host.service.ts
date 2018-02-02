import {Injectable, EventEmitter} from '@angular/core';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/Rx';

import {environment} from '../../../environments/environment';
import {User} from '../../shared/models/user.model';
import {Availability} from './../models/availability.model';

@Injectable()
export class HostService {
    public  selectedHostChanged$: EventEmitter<User> = new EventEmitter();
    
    constructor(private http: HttpClient) {
    }

    public HostAvailabilityList(startDate: Date, hostId: number, endDate?: Date, roomId?: number) {
        const url = environment.apiUrl + '/availability/list';
        let params = new HttpParams();
        
        let x: Date;
        x = startDate;
        params = params.append("startDate", new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate(), 0, 0, 0)).toJSON());

        if (endDate) {
            x = endDate;
            params = params.append("endDate", new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate(), 23, 59, 59)).toJSON());
        }
        if (roomId > 0) {
            params = params.append("roomId", roomId.toString());
        }
        if (hostId > 0) {
            params = params.append("hostId", hostId.toString());
        }

        const body = JSON.stringify(params);
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url, {headers: headers, withCredentials: true, params: params});
    }

    public HostList() {
        const url = environment.apiUrl + '/availability/host/list';
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url, { headers: headers, withCredentials: true });
    }

    public selectHost(host: User) {
        this.selectedHostChanged$.emit(host);
    }  
}