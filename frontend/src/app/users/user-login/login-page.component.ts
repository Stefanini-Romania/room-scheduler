import {Component} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'login-component',
    template: `
        <login-form></login-form>
        <rs-footer></rs-footer>
    `,
    styleUrls: [],
    providers: [NgbActiveModal],
})

export class LoginPageComponent {
    
}
//removed <login-form></login-form>
