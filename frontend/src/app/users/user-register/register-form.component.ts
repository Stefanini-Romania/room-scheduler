import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {Observable} from 'rxjs/Observable';
import {HttpClient, HttpHeaders, HttpParams} from '@angular/common/http';

import {environment} from './../../../environments/environment';
import {RoleIdEnum} from './../../shared/models/roleId.model';
import {DepartmentIdEnum} from './../../shared/models/departmentIdEnum.model';
import {User} from '../../shared/models/user.model';
import {AuthService} from '../../auth/shared/auth.service';

@Component({
    moduleId: module.id,
    selector: 'register-form',
    templateUrl: './register-form.component.html',
    styleUrls: [],
    providers: [AuthService],
})

export class RegisterFormComponent {
    public model: User = {      
        firstName: '',
        lastName: '',
        name: '',
        email: '',
        password: '',    
        departmentId: 1,
        roleId: 1
    };
    
    public errorMessage: string = '';
    
    DepartmentIdEnum: DepartmentIdEnum[] = [];
    RoleIdEnum: RoleIdEnum[] = [];

    constructor(private authService: AuthService, private router: Router, private http: HttpClient) {
    }

    register(event: User) {
        const url = environment.apiUrl + '/users/add';
        const body = JSON.stringify({
            firstName: this.model.firstName.toLocaleString(),
            lastName: this.model.lastName.toLocaleString(),
            name: this.model.name.toLocaleString(),
            email: this.model.email.toLocaleString(),
            password: this.model.password.toLocaleString(),
            departmentId: this.model.departmentId,
            roleId: this.model.roleId = 1
            
        });
        const headers = new HttpHeaders().set('Content-Type', 'application/json; charset=utf-8');

        return this.http.post(url, body, {headers: headers, withCredentials: true})
        .catch((error: any) => {
            return Observable.throw(error);
        })
        .map((response: Response) => {
            return response;
        });

        /*this.userService.create(this.model)
            .subscribe(
                () => {
                    this.router.navigate(['/calendar']);
                },
                error => {
                    this.errorMessage = error.error.message;
                });*/

    }

   
}