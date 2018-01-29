import {Injectable, EventEmitter} from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/Rx';

import {environment} from '../../../environments/environment';
import {User} from '../../shared/models/user.model';

@Injectable()
export class HostService{
    public  selectedHostChanged$: EventEmitter<User> = new EventEmitter();
    
    constructor(private http: HttpClient) {
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