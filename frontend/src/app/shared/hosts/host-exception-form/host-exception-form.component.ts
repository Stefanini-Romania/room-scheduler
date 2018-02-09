import {Component, NgModule, Output, EventEmitter} from '@angular/core'
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
    @Output()
    successfullAddException = new EventEmitter;

    public model: Availability = <Availability>{};
    public startHour;
    public endHour;
    public displayDate = new Date();
    public minuteStep = 30;

    constructor(public hostService: HostService, private formBuilder: FormBuilder, private translate: TranslateService, public activeModal: NgbActiveModal, private toastr: ToastrService){}


    daysOfWeek = [
        {name:this.translate.instant("calendar.days.namesAbbr.Mon"), value:1, checked:false},
        {name:this.translate.instant("calendar.days.namesAbbr.Tue"), value:2, checked:false},
        {name:this.translate.instant("calendar.days.namesAbbr.Wed"), value:3, checked:false},
        {name:this.translate.instant("calendar.days.namesAbbr.Thu"), value:4, checked:false},
        {name:this.translate.instant("calendar.days.namesAbbr.Fri"), value:5, checked:false}
    ]

    get selectedDaysOfWeek() {
        return this.daysOfWeek
            .filter(opt => opt.checked)
            .map(opt => opt.value)
    }

    addException(){
        JSON.stringify(this.model.startDate);
        let availabilityStartDate = new Date(this.model.startDate["year"], this.model.startDate["month"]-1, this.model.startDate["day"], this.startHour["hour"], this.startHour["minute"], 0);
        JSON.stringify(availabilityStartDate);

        let dayOfWeek = availabilityStartDate.getDay();
        let currentDate = availabilityStartDate.getDate();
        while(dayOfWeek!==1){
            dayOfWeek--;
            currentDate=currentDate-1;
        }
        availabilityStartDate.setDate(currentDate);

        this.model.endDate =new Date(availabilityStartDate.getFullYear(), availabilityStartDate.getMonth(),availabilityStartDate.getDate(),  this.endHour["hour"], this.endHour["minute"]);
        JSON.stringify(this.model.endDate)
        this.model.daysOfWeek = this.selectedDaysOfWeek;

        this.hostService.AddHostException(
            availabilityStartDate,
            this.model.endDate,
            this.model.daysOfWeek, 
            this.model.hostId = 3).subscribe(() => {                                  
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
}
