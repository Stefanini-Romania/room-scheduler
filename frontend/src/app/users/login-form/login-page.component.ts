import {Component} from '@angular/core';
import {Router} from '@angular/router';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'login-component',
    template: `
        <login-form (successfullLogin)="onSuccessfullLogin()"></login-form>
        <rs-footer></rs-footer>
    `,
    styleUrls: [],
    providers: [NgbActiveModal],
})

export class LoginPageComponent {
    constructor(private router: Router) {
    }
    
    onSuccessfullLogin() {
        return this.router.navigate(['/calendar']);
    }
}

