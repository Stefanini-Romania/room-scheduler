import {Component, NgModule, OnInit} from '@angular/core'
import {TranslateService} from "@ngx-translate/core";
import {NgbActiveModal, NgbModalRef, NgbModal} from '@ng-bootstrap/ng-bootstrap'
import {FormBuilder, FormGroup} from '@angular/forms';

import {User} from '../../models/user.model';
import {Availability} from '../../models/availability.model';
import {HostAvailabilityService} from '../../services/host-availability.service';

@Component({
    selector: 'host-availability-form',
    templateUrl: './host-availability-form.component.html',
    providers: [HostAvailabilityService]
})

export class HostAvailabilityForm{
    public checkboxGroupForm: FormGroup;

    constructor( private formBuilder: FormBuilder, private translate: TranslateService, public activeModal: NgbActiveModal){}

    ngOnInit() {
        this.checkboxGroupForm = this.formBuilder.group({
          monday: false,
          tuesday: false,
          wednesday: false,
          thursday: false,
          friday: false
        });
      }
}