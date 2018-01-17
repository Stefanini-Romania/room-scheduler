import {Router} from '@angular/router';
import {Component, EventEmitter, Output} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import {TranslateService} from '@ngx-translate/core';

import {User} from '../../shared/models/user.model';
import {UserService} from '../shared/users.service';




@Component({
    selector: 'lost-password-form',
    templateUrl: './lost-password-form.component.html',
    styleUrls: [],
    providers: [],
})

export class LostPasswordFormComponent{
    model: User = <User> {};
    public errorMessages: any = {};

    @Output()
    emailSent = new EventEmitter;

    constructor(public router: Router, public userService: UserService, private toastr: ToastrService, private translate: TranslateService) {
    }

    sendMail(email){
        this.userService.mailPassReset(this.model.email).subscribe(
            () => {},
            response => {
                if (response.status == 200) {
                    this.toastr.success(
                        this.translate.instant('email.sent'), '',
                        {positionClass: 'toast-bottom-right'}
                    ); 
                    this.emailSent.emit();
                } 
                else {
                    this.errorMessages = response.error.message;               
                }
            });       
    }

    changePassword(password){
        this.userService.resetPassword(this.model.password).subscribe(
            () => {

        //     },
        //     error => {
                
        //     });       
    });
}
       
}