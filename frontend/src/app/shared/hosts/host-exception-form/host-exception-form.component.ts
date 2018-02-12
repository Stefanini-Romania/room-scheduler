import {Component, NgModule, Input, Output, EventEmitter} from '@angular/core'
import {TranslateService} from "@ngx-translate/core";
import {NgbActiveModal, NgbModalRef, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {FormBuilder, FormGroup} from '@angular/forms';
import {SelectControlValueAccessor} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';

import {User} from '../../models/user.model';
import {Availability} from '../../models/availability.model';
import {HostService} from './../../services/host.service';

@Component({
    selector: 'host-exception-form',
    templateUrl: './host-exception-form.component.html',
    providers: [HostService]
})

export class HostExceptionForm {

    @Input() 
    host: User;

    @Output()
    successfullAddException = new EventEmitter;

    public model: Availability = <Availability>{};
    public startHour;
    public endHour;
    public displayDate = new Date();
    public minuteStep = 30;
    public addExcept: boolean;

    constructor(public hostService: HostService, private formBuilder: FormBuilder, private translate: TranslateService, public activeModal: NgbActiveModal, private toastr: ToastrService){}

    addException(){
        if(this.model.startDate && this.startHour && this.endHour) {
            JSON.stringify(this.model.startDate);
            let availabilityStartDate = new Date(this.model.startDate["year"], this.model.startDate["month"]-1, this.model.startDate["day"], this.startHour["hour"], this.startHour["minute"], 0);
            JSON.stringify(availabilityStartDate);

            this.model.endDate =new Date(availabilityStartDate.getFullYear(), availabilityStartDate.getMonth(),availabilityStartDate.getDate(),  this.endHour["hour"], this.endHour["minute"]);
            JSON.stringify(this.model.endDate)

            this.hostService.AddHostException(
                availabilityStartDate,
                this.model.endDate,
                this.host.id).subscribe(() => {                                  
                },
                error => {
                    if(error.status==200) {
                        this.successfullAddException.emit();
                        this.toastr.success(
                            this.translate.instant('Exception.successfully.added'), '',
                            {positionClass: 'toast-bottom-right'}
                        );
                    }
                    else {
                        this.toastr.warning(
                            this.translate.instant('Exception.notSuccessfull'), '',
                            {positionClass: 'toast-bottom-right'}
                        );
                    }
                });    
        } 
        else this.addExcept=false;
    }
}
