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
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.get(url, { headers: headers, withCredentials: true });
    }

    public createUser(firstName: string, lastName: string, email: string, password: string, departmentId: number,  userRole?: RoleEnum[]) {
        const url = environment.apiUrl + '/user/register';
        let body = JSON.stringify({
            firstName: firstName, 
            lastName: lastName,
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

    public editUser(id: number, firstName: string, lastName: string, email: string, departmentId: number,  userRole?: RoleEnum[], isActive?: boolean, password?: string) {
        const url = environment.apiUrl + '/user/edit/' + id;
        const body = JSON.stringify({
            id: id,
            firstName: firstName,
            lastName: lastName,
            userRole: userRole,
            email: email,       
            departmentId: departmentId,
            isActive: isActive,
            password: password
        });

        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.put(url, body, { headers: headers, withCredentials: true })
            .catch((error: any) => {
                return Observable.throw(error);
            })
            .map((response: Response) => {
                return response;
            });
    }

    public mailPassReset(email: string) { 
        const url = environment.apiUrl + '/email/resetpass/' + email;
        const body = JSON.stringify({
            email: email});
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, body)
            .catch((error: any) => {
                return Observable.throw(error);
            })
            .map((response: Response) => {
                return response;
            });
    }

    public checkCodeResetPass(resetPassCode) { 
        const url = environment.apiUrl + '/user/resetpass/' + resetPassCode;
        const body = JSON.stringify({
            resetPassCode: resetPassCode});
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.post(url, body)
            .catch((error: any) => {
                return Observable.throw(error);
            })
            .map((response: Response) => {
                return response;
            });
    }

    public resetPassword(resetPassCode: string, password: string) {
        const url = environment.apiUrl + '/user/resetpass/' + resetPassCode;
        const body = JSON.stringify({
            password: password
        });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');
        return this.http.put(url, body)
            .catch((error: any) => {
                return Observable.throw(error);
            })
            .map((response: Response) => {
                return response;
            });
    }
}
