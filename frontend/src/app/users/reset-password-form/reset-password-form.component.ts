import {Router} from '@angular/router';
import {Component, EventEmitter, Output} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import {TranslateService} from '@ngx-translate/core';

import {User} from '../../shared/models/user.model';
import {UserService} from '../shared/users.service';


@Component({
    selector: 'reset-password-form',
    templateUrl: './reset-password-form.component.html',
    styleUrls: [],
    providers: [],
})

export class ResetPasswordFormComponent{
    model: User = <User> {};
    public errorMessages: any = {};

    @Output()
    emailSent = new EventEmitter;

    constructor(public router: Router, public userService: UserService, private toastr: ToastrService, private translate: TranslateService) {
    }

    sendMail(email){
        this.userService.mailPassReset(this.model.email).subscribe(
            () => {},
          
            error => {
                if (error.status == 200) {
                    this.toastr.success(
                        this.translate.instant('email.sent'), '',
                        {positionClass: 'toast-bottom-right'}
                    ); 
                    this.emailSent.emit();
                } 
                else {
                           
                    this.errorMessages = error.error.message; 
                //     if (error.status == 200) {
                //         this.toastr.success(
                //             this.translate.instant('email.sent'), '',
                //             {positionClass: 'toast-bottom-right'}
                //         ); 
                //         this.emailSent.emit();
                //     } 
                //     else {
                //         this.errorMessages = {'generic': [error.error.message]};
                //         // build error message
                //         for (let e of error.error.errors) {
                //             let field = 'generic';
                            
                //             if (['Email'].indexOf(e.field) >= 0) {
                //                 field = e.field;
                //             }
                //             if (!this.errorMessages[field]) {
                //                 this.errorMessages[field] = [];
                //             }   
                //             this.errorMessages[field].push(e.errorCode);
                //         }               
                //     }
                // });                     
                }
            });       
    }

    changePassword(password){  
        this.userService.resetPassword(this.model.email, this.model.password).subscribe(
            () => {},

            error => {              
                if (error.status == 200) {
                    this.toastr.success(
                        this.translate.instant('password.changed'), '',
                        {positionClass: 'toast-bottom-right'}
                    ); 
                    this.emailSent.emit();
                } 
                else {
                    this.errorMessages = error.error.message; 
                }
            });             
    }
}
