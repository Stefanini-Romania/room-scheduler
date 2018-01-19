import {Injectable, Injector, EventEmitter, Output} from '@angular/core';
import {HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {Router} from '@angular/router';
import {Observable} from 'rxjs/Observable';

import {AuthService} from './auth.service';
import {User} from './../../shared/models/user.model';

@Injectable()
export class APIRequestInterceptor implements HttpInterceptor {

 
   public removeUser$: EventEmitter<User> = new EventEmitter();

    constructor(private router: Router, private injector: Injector){}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const authService = this.injector.get(AuthService);
        return next.handle(req).do(event => {}, err => {
            if (err instanceof HttpErrorResponse && err.status == 401) {           
                sessionStorage.removeItem('currentUser');
                //this.router.navigate(['/login']);  
                window.location.reload(); //FIX
                this.removeUser$.emit(null);                              
            } 
        });      
    }           
}