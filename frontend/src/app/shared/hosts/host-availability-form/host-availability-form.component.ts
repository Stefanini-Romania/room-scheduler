import {Component, NgModule} from '@angular/core'
import {TranslateService} from "@ngx-translate/core";
import {NgbActiveModal, NgbModalRef, NgbModal} from '@ng-bootstrap/ng-bootstrap'

import {User} from '../../models/user.model';
import {Availability} from '../../models/availability.model';
import {HostAvailabilityService} from '../../services/host-availability.service';

@Component({
    selector: 'host-availability-form',
    templateUrl: './host-availability-form.component.html',
    providers: [HostAvailabilityService]
})

export class HostAvailabilityForm{

    constructor(private translate: TranslateService, public activeModal: NgbActiveModal){}

}