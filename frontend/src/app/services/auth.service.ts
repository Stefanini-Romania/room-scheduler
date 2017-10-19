import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
//import {AppServices} from "../../app.services";
import {Router} from "@angular/router";
// import 'rxjs/add/operator/toPromise';
// import {Observable} from 'rxjs/Rx';

@Injectable()
export class AuthService {
    constructor(private http: Http) { };

    authenticate (username: string, password: string) {
        return this.http.post('/api/authenticate', JSON.stringify({ username: username, password: password }));
        /*.then((response: Response) => {
            // login successful if there's a jwt token in the response
            let user = response.json();
            if (user && user.token) {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(user));
            }

            return user;
        });*/
    }
};
