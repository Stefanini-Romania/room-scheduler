import {environment} from '../../../environments/environment';
import {Settings} from '../../shared/models/settings.model';

import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {Observable} from 'rxjs/Observable';
import 'rxjs/Rx';

@Injectable()
export class SystemParametersService{

    constructor(private http: HttpClient){}

    public listParameters() {
        const url = environment.apiUrl + '/settings/list';
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url, { headers: headers, withCredentials: true });
    }

    public editParameters() { //NOT FINISHED
        const url = environment.apiUrl + '/settings/session/edit/{value}';
        const body = JSON.stringify ({
        });

       
        return this.http.post(url, body);
    }
    
} 