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
        alert('adf');
        // return this.http.post('/api/authenticate', JSON.stringify({ username: username, password: password }));
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

// @Injectable()
// export class AuthenticationService {
//   public token: any;

//   constructor(
//     private http: Http,
//     private appService: AppServices,
//     private router: Router
//   ) {
//     this.token = localStorage.getItem('token');
//   }

//   login(currentUser: string, password: string): Observable<boolean> {
//     let headers = new Headers();
//     let body = null;
//     headers.append("Authorization",("Basic " + btoa(username + ':' + password)));

//     return this.http.post(this.appService.api + '/auth.service', body, {headers: headers})
//       then(response: Response) => {
//         let token = response.json() && response.json().token;
//         if (token) {
//           this.token = token;
//           localStorage.setItem('Conform_token', token);
//           return true;
//         } else {
//           return false;
//         }
//       });
//   }

//   logout(): void {
//     this.token = null;
//     localStorage.removeItem('Conform_token');
//     this.router.navigate(['/login.component.html']);
//   }
// }