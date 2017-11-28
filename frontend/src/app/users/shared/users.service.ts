import {environment} from '../../../environments/environment';
import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import 'rxjs/Rx';
import {Observable} from 'rxjs/Observable';
import {User} from "../../shared/models/user.model";

@Injectable()
export class UserService {

    constructor(private http: HttpClient) {
    }

    public listUsers() {
        const url = environment.apiUrl + '/users/list';
        
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url);

    }

   /*  private createUser(user: User) {
        const url = environment.apiUrl + '/user/create';
        const body = JSON.stringify({
            name:user.name,
            firstName:user.firstName,
            lastName:user.lastName,
            userRole:user.userRole,
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

    private editUser(user: User) {
        const url = environment.apiUrl + '/user/edit/' + user.id;
        const body = JSON.stringify({
            name:user.name,
            firstName:user.firstName,
            lastName:user.lastName,
            userRole:user.userRole,
        });

        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.put(url, body, { headers: headers, withCredentials: true })
            .catch((error: any) => Observable.throw(error.message))
            .map((response: Response) => {
                return response;
            });
    }

    public save(user: User) {
        return user.id ? this.editUser(user) : this.createUser(user);
    }
*/
}