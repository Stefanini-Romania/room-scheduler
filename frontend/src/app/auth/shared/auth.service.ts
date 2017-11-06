import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { HttpClient, HttpHeaders, HttpRequest } from '@angular/common/http';
import { User } from '../../shared/user.model';
import {Jsonp, URLSearchParams } from '@angular/http';
import 'rxjs/Rx';
import { Observable } from 'rxjs/Observable';
import { map } from 'rxjs/operator/map';

@Injectable()
export class AuthService {
    
    users: User[];
    
    constructor(private http: HttpClient) {}

    authenticate(name: string, password: string) {
        const url = 'http://172.25.4.165:88/api/auth/login';
        const body = JSON.stringify({ name: name, password: password });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, body, { headers: headers })
        
                        .catch((error:any) => Observable.throw(error.message))
                        .map((response: Response) => {
            
                                if (response) {
                    
                                    sessionStorage.setItem('currentUser', JSON.stringify(response));
                                }
                                return response; 

        })
    }

    logout() {
        sessionStorage.removeItem('currentUser');
    }

    getLoggedUser(): User {
        const sessionData = sessionStorage.getItem('currentUser');
        let u: User = null;
        if (sessionData && sessionData !== {}) {
            u = Object.assign(new User, JSON.parse(sessionData));
        }
        return u;
    }

    isLoggedIn (): boolean {
        return this.getLoggedUser() instanceof User;
    }
}
