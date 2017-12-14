import {Injectable, EventEmitter} from '@angular/core';
import {Response} from '@angular/http';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs/Observable';
import {environment} from '../../../environments/environment';
import 'rxjs/Rx';

import {User} from '../../shared/models/user.model';

@Injectable()
export class AuthService {
    
    public user$: EventEmitter<User> = new EventEmitter();

    constructor(private http: HttpClient) {
    }

    authenticate(name: string, password: string) {
        const url = environment.apiUrl + '/api/auth/login';
        const body = JSON.stringify({name: name, password: password});
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, body, {headers: headers, withCredentials: true})
            .catch((error: any) => Observable.throw(error))
            .map((response: Response) => {
                if (response) {
                    sessionStorage.setItem('currentUser', JSON.stringify(response));
                    this.user$.emit(this.getLoggedUser());
                }

                return response;
            });
    }

    logout() {

        sessionStorage.removeItem('currentUser');
        this.user$.emit(null);

        const url = environment.apiUrl + '/api/auth/logout';
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, { headers: headers, withCredentials: true }); 
    }

    getLoggedUser(): User {
        const sessionData = sessionStorage.getItem('currentUser');
        let u: User = null;
        if (sessionData && sessionData != null) {
            u = Object.assign(new User, JSON.parse(sessionData));
        }

        return u;
    }

    isLoggedIn(): boolean {
        return this.getLoggedUser() instanceof User;
    }
}
