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

    public HostAvailabilityList(startDate: Date, hostId?: number, roomId?: number) {
        const url = environment.apiUrl + '/availability/list';
        let params = new HttpParams();
        
        let x: Date;
        x = startDate;
        params = params.append("startDate", new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate(), 0, 0, 0)).toJSON());
        if (hostId > 0) {
            params = params.append("hostId", hostId.toString());
        }

        if (roomId > 0) {
            params = params.append("roomId", roomId.toString());
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

    public AddHostAvailability(startDate: Date, endDate: Date, availabilityType: number, daysOfWeek: any, occurrence: number, roomId?: number, hostId?: number){
        const url = environment.apiUrl + '/availability/add';
        let params = new HttpParams();
        if (hostId > 0){
            params = params.append("hostId", hostId.toString());
        }
        const body = JSON.stringify({
            startDate: startDate.toLocaleString(),
            endDate: endDate.toLocaleString(),
            availabilityType: availabilityType,
            daysOfWeek: daysOfWeek,
            occurrence: occurrence,
            roomId: roomId,
            
        });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

        return this.http.post(url, body, {headers: headers, withCredentials: true, params: params})
            .catch((error: any) => {
                return Observable.throw(error);
            })
            .map((response: Response) => {
                return response;
            });
    }

    public EditHostAvailability(id: number, startDate: Date, endDate: Date, status: number, occurrence: number, roomId?: number){
        const url = environment.apiUrl + '/availability/edit/' + id;
        const body = JSON.stringify({
            startDate: startDate.toLocaleString(),
            endDate: endDate.toLocaleString(),
            status: status,
            occurrence: occurrence, 
            roomId: roomId,       
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

    public AddHostException(startDate: Date, endDate: Date, hostId?: number){
        const url = environment.apiUrl + '/availability/exception/add';
        let params = new HttpParams();
        if (hostId > 0){
            params = params.append("hostId", hostId.toString());
        }
        const body = JSON.stringify({
            startDate: startDate,
            endDate: endDate,             
        });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

        return this.http.post(url, body, {headers: headers, withCredentials: true, params: params})
            .catch((error: any) => {
                return Observable.throw(error);
            })
            .map((response: Response) => {
                return response;
            });
    }  
}