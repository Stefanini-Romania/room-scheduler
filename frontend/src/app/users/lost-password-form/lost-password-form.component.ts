import {Router} from '@angular/router';
import {Component} from '@angular/core';

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

    constructor(public router: Router, public userService: UserService) {
    }

    sendMail(){
        // this.userService.mailPassReset(this.model.email).subscribe(
        //     () => {

        //     },
        //     error => {
        //         //this.errorMessages = error.error.message;
        //     });       
    }

    changePassword(){
        this.userService.resetPassword(this.model.password).subscribe(
            () => {

            },
            error => {
                
            });       
    }
       
}