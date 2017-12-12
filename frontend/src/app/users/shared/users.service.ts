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

    public listUsers() {
        const url = environment.apiUrl + '/user/list';
        return this.http.get(url);
    }

    public createUser(firstName: string, lastName: string, name: string, email: string, password: string, departmentId: number,  userRoles?: RoleEnum[]) {
        const url = environment.apiUrl + '/user/add';
        let body = JSON.stringify({
            firstName: firstName, 
            lastName: lastName,
            name: name,
            email: email,
            password: password,
            departmentId: departmentId,
            userRole: [RoleEnum.attendee]         
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

    // public deleteUser(user: User) {
    //     const url = environment.apiUrl + '/user/delete/' + user.id;
    //     console.log(url);
    //     const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

    //     return this.http.delete(url, {headers: headers, withCredentials: true})
    //         .catch((error: any) => {               
    //             return Observable.throw(error.message);
    //         })
    //         .map((response: Response) => {        
    //             return response;
    //         });
    // }


    public editUser(id: number, firstName: string, lastName: string, name: string, email: string, departmentId: number,  userRoles?: RoleEnum[], isActive?: boolean, password?: string) {
        const url = environment.apiUrl + '/user/edit/' + id;
        const body = JSON.stringify({
            name: name,
            firstName: firstName,
            lastName: lastName,
            userRole: [RoleEnum.attendee],
            email: email,       
            departmentId: departmentId,
            isActive: isActive,
            password: password
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