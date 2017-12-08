import {environment} from '../../../environments/environment';
import {User} from "../../shared/models/user.model";

import {Injectable} from '@angular/core';
import {Response} from '@angular/http';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';
import {RoleEnum} from '../../shared/models/role.model';
import {Observable} from 'rxjs/Observable';
import 'rxjs/Rx';


@Injectable()
export class UserService {

    public user: User;
    
    constructor(private http: HttpClient) {
    }

    public listUsers(limit?: number, page?: number) {
        limit = 8;
        page = 1;
        const url = environment.apiUrl + '/users/list';
        let params = new HttpParams();
        params = params.append("limit", limit.toString());
        params = params.append("page",page.toString());

        const body = JSON.stringify(params);
        return this.http.get(url, {params: params});
    }

    public createUser(firstName: string, lastName: string, name: string, email: string, password: string, departmentId: number,  userRoles?: RoleEnum[]) {
        const url = environment.apiUrl + '/users/add';
        let body = JSON.stringify({
            firstName: firstName, 
            lastName: lastName,
            name: name,
            email: email,
            password: password,
            departmentId: departmentId,
            userRole: userRoles
                    
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

    public deleteUser(user: User) {
        const url = environment.apiUrl + '/users/delete/' + user.id;
        console.log(url);
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

        return this.http.delete(url, {headers: headers, withCredentials: true})
            .catch((error: any) => {               
                return Observable.throw(error.message);
            })
            .map((response: Response) => {        
                return response;
            });
    }


    public editUser(id: number, firstName: string, lastName: string, name: string, email: string, password: string, departmentId: number,  userRoles?: RoleEnum[]) {
        const url = environment.apiUrl + '/user/edit/' + id;
        const body = JSON.stringify({
            name:name,
            firstName:firstName,
            lastName:lastName,
            userRole:userRoles,
            email:email,
            password: password,
            departmentId: departmentId,

        });

        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.put(url, body, { headers: headers, withCredentials: true })
            .catch((error: any) => Observable.throw(error.message))
            .map((response: Response) => {
                return response;
            });
    }

    // public save(user: User) {
    //     return user.id ? this.editUser(user) : this.createUser(user);
    // }

}