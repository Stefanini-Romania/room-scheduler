import {Component} from '@angular/core';
import {RegisterFormComponent} from '../user-register/register-form.component';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'login-component',
    templateUrl: './log
    template: `
        <login-form></login-form>
        <rs-footer></rs-footer>
    `,
    styleUrls: [],
    providers: [NgbActiveModal],
})

export class LoginPageComponent {
}
