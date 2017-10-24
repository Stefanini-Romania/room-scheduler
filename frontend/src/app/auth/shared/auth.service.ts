import { Injectable } from '@angular/core';
//import { Http, Headers, Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/Rx';
import { User } from '../../shared/user.model';
import {Jsonp, URLSearchParams } from '@angular/http';

@Injectable()
export class AuthService {
    
    /* constructor(private jsonp: Jsonp) { };
    private authUrl = 'http://localhost:4200/calendar';
      
    authenticate(UserName: string, Password: string) {  
        const endPoint = 'user.find'  
        let params = new URLSearchParams();  
        params.set('key', '555f8155d42d5c9be4705beaf4cce089');  
        params.set('username', UserName);  
        params.set('password', Password);  
        params.set('format', 'json');  
        params.set('callback', 'JSONP_CALLBACK');  
        // Returns response  
        return this.jsonp  
              .get(this.authUrl + endPoint, { search: params })  
              .map(response => <User[]> response.json().userfinder.users.user);  
    }  
    */

    constructor(private http: HttpClient) { }

    login(username: string, password: string) {
        console.log('here');
        this.http.get('http://google.com/api/some-authenticate')
        .catch((error:any) => Observable.throw(error.message));
        return this.http.post('/api/authenticate', JSON.stringify({ username: username, password: password }))
        /*.map((response: Response) => {
            // login successful if there's a jwt token in the response
            let user = response.json();
            if (user && user.token) {
                // store user details and jwt token in local storage to keep user logged in between page refreshes
                localStorage.setItem('currentUser', JSON.stringify(user));
            }

            return user;
        })*/;
    } 
}
