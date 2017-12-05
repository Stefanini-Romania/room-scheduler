import {environment} from '../../../environments/environment';
import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import 'rxjs/Rx';
import {Observable} from 'rxjs/Observable';

import {User} from '../../shared/models/user.model';


@Injectable()
export class RegisterService {

    constructor(private http: HttpClient) {
    }

     public createUser(firstName: string, lastName: string, name: string, email: string, password: string, departmentId: number, roleId? :number) {
        const url = environment.apiUrl + '/users/add';
        const body = JSON.stringify({
            firstName: firstName, 
            lastName: lastName,
            name: name,
            email: email,
            password: password,
            departmentId: departmentId,
            roleId: roleId = 1         
        });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

        return this.http.post(url, body, {headers: headers, withCredentials: true})
        .catch((error: any) => {
            return Observable.throw(error);
        })
        .map((response: Response) => {
            return response;
        });

    }
}
