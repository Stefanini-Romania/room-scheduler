import {Component} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';


@Component({
    selector: 'register-component',
    template: `
        <register-form></register-form>
        <rs-footer></rs-footer>
    `,
    styleUrls: [],
    providers: [NgbActiveModal]

})

export class RegisterPageComponent {
   
}