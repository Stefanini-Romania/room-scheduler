import {Injectable, EventEmitter} from '@angular/core';
import {Response} from '@angular/http';
import {TranslateService} from '@ngx-translate/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs/Observable';
import {environment} from '../../../environments/environment';
import 'rxjs/Rx';

import {User} from '../../shared/models/user.model';
import {LanguageSelector} from '../../core/language-selector/language-selector';

@Injectable()
export class AuthService {
    
    public user$: EventEmitter<User> = new EventEmitter();

    constructor(private http: HttpClient, private translate: TranslateService) {
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

    // intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    //     return next.handle(req).do(event => {}, err => {
    //         if (err instanceof HttpErrorResponse && err.status == 401) {
               
    //             this.logout();

    //         }
    //     });
    // }

    logout() {
        const url = environment.apiUrl + '/api/auth/logout';
        return this.http.get(url, { responseType: 'text', withCredentials: true }).subscribe(
            () => {
                sessionStorage.removeItem('currentUser');
                this.user$.emit(null);

            },
            error => {
                throw error;
            });
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
