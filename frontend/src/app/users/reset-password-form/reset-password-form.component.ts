import {Router, ActivatedRoute } from '@angular/router';
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
    resetPassCode: string;
    checkIfloginRoute = false;
    public errorMessages: any = {};
    public email;
    public password;
    public empty: boolean;
    
    @Output()
    emailSent = new EventEmitter;

    constructor(public router: Router, public userService: UserService, private toastr: ToastrService, private translate: TranslateService, route: ActivatedRoute ) {   
        // TODO: Refactor using an angulat service or smt
        let part = window.location.href.split('/');
        this.resetPassCode = part[4];
    
        //let paramCode = params.get("resetPassCode");
        if (part[4]) { 
            this.userService.checkCodeResetPass(part[4]).subscribe(
                () => {},
                error => {
                    if (error.status == 404) {
                        this.toastr.warning(
                            this.translate.instant('password.notChanged'), '',
                            {positionClass: 'toast-bottom-right'}
                        ); 
                        this.router.navigate(['/resetpass']);                
                    }
                });
        }
    }

    sendMail(email){
        if(this.model.email){
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
                    if (error.status == 404) {
                        this.toastr.warning(
                            this.translate.instant('email.notSent'), '',
                            {positionClass: 'toast-bottom-right'}
                        );
                    }
                }); 
        }
        else this.empty=true;
              
    }

    changePassword(password){  
        this.userService.resetPassword(this.resetPassCode, this.model.password).subscribe(
            () => {
                this.toastr.success(
                    this.translate.instant('password.changed'), '',
                    {positionClass: 'toast-bottom-right'}
                );  
                this.router.navigate(['/login']);       
            },
            error => {                        
                this.errorMessages = {'generic': [error.error.message]};
                for (let e of error.error.errors) {
                    let field = 'generic';
                    
                    if (['Password'].indexOf(e.field) >= 0) {
                        field = e.field;
                    }
                    if (!this.errorMessages[field]) {
                        this.errorMessages[field] = [];
                    }   
                    this.errorMessages[field].push(e.errorCode);
                }                      
            });
    } 
    
}
