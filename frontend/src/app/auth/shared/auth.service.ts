import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {User} from '../../shared/models/user.model';
import 'rxjs/Rx';
import {Observable} from 'rxjs/Observable';
import {Router} from '@angular/router';

@Injectable()
export class AuthService {

    users: User[];

    constructor(private http: HttpClient, private router: Router) {
    }

    authenticate(name: string, password: string) {
        const url = 'http://172.25.4.165:88/api/auth/login';
        const body = JSON.stringify({name: name, password: password});
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, body, {headers: headers})
            .catch((error: any) => Observable.throw(error))
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
        if (sessionData && sessionData != null) {
            u = Object.assign(new User, JSON.parse(sessionData));
        }
        return u;
    }

    isLoggedIn(): boolean {
        return this.getLoggedUser() instanceof User;
    }

    notLoggedIn() {
        if(!(this.isLoggedIn())) 
        {
            alert("You need to login if you want to make an appointment!");
            this.router.navigate(['/login']);
        }
    }
}
