import {Component, Input, NgModule, OnInit, Output, EventEmitter} from '@angular/core'
import {TranslateService} from "@ngx-translate/core";
import {NgbActiveModal, NgbModalRef, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ToastrService} from 'ngx-toastr';
import {FormBuilder, FormGroup, FormControl} from '@angular/forms';
import { SelectControlValueAccessor } from '@angular/forms';
import {NgbDatepickerConfig, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';

import {User} from '../../models/user.model';
import {Availability} from '../../models/availability.model';
import {HostService} from './../../services/host.service';
import {Room} from '../../models/room.model';


@Component({
    selector: 'host-availability-form',
    templateUrl: './host-availability-form.component.html',
    providers: [NgbDatepickerConfig]
})

export class HostAvailabilityForm{
    @Input() host: User;

    @Output()
    successfullAddAvailability = new EventEmitter;
    successfullEditAvailability = new EventEmitter;

    public model: Availability = <Availability>{};
    public startHour;
    public endHour;
    public selectedRoom: Room;
    public selectedHost: User;
    public addAvail: boolean;
    public minuteStep = 30;
    
    constructor( private formBuilder: FormBuilder, private translate: TranslateService, public activeModal: NgbActiveModal, private hostService: HostService, private toastr: ToastrService, private datePickerConfig: NgbDatepickerConfig){
        datePickerConfig.markDisabled = (date: NgbDateStruct) => {
            const day = new Date(date.year, date.month - 1, date.day);
            return day.getDay() === 0 || day.getDay() === 6;
          };

        
    }
    //validation for hours between 9 and 18 availability
    ctrl = new FormControl('', (control: FormControl) => {
        const value = control.value;
    
        if (!value) {
          return null;
        }
    
        if (value.hour < 9 || value.hour > 17) {
          return {tooEarly: true};
        }
    
        return null;
    });
 
    daysOfWeek = [
        {name:this.translate.instant("calendar.days.namesAbbr.Mon"), value:1, checked:false},
        {name:this.translate.instant("calendar.days.namesAbbr.Tue"), value:2, checked:false},
        {name:this.translate.instant("calendar.days.namesAbbr.Wed"), value:3, checked:false},
        {name:this.translate.instant("calendar.days.namesAbbr.Thu"), value:4, checked:false},
        {name:this.translate.instant("calendar.days.namesAbbr.Fri"), value:5, checked:false}
    ]
    
    get selectedDaysOfWeek() { // right now: ['1','3']
        return this.daysOfWeek
            .filter(opt => opt.checked)
            .map(opt => opt.value)
    }

    occurrence = [{value: 1}, {value: 2}, {value: 3}, {value: 4}];
    selectedOccurrence = this.occurrence[0];

    onRoomChanged(selectedRoom: Room) {
        this.selectedRoom = selectedRoom;
    }

    addAvailability(){
        if(this.model.startDate && this.startHour && this.endHour){
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
    
            this.model.endDate =new Date(availabilityStartDate.getFullYear(), availabilityStartDate.getMonth(),availabilityStartDate.getDate(),  this.endHour["hour"], this.endHour["minute"]) ;
        
            JSON.stringify(this.model.endDate)
            
            this.model.daysOfWeek = this.selectedDaysOfWeek;
            this.model.occurrence=this.selectedOccurrence.value;
            this.hostService.AddHostAvailability(
    
                  availabilityStartDate,
                  this.model.endDate, 
                  this.model.availabilityType = 0, 
                  this.model.daysOfWeek, 
                  this.model.occurrence, 
                  this.selectedRoom.id,
                  this.host.id).subscribe(() => {
                   
                                                                             
                },
                error => {
                    if(error.status==200){
                        this.successfullAddAvailability.emit();
                        this.toastr.success(
                            this.translate.instant('Availability.successfully.added'), '',
                            {positionClass: 'toast-bottom-right'}
                        );
                    }
    
                    else{
                        this.toastr.warning(
                            this.translate.instant('Availability.notSuccessfull'), '',
                            {positionClass: 'toast-bottom-right'}
                        );
                    }
                });        
        }
        else this.addAvail=false;
    }

    editAvailability() {
        this.hostService.EditHostAvailability(this.model.id, this.model.startDate, this.model.endDate, this.model.status, this.model.occurrence, this.model.roomId).subscribe(
            () => {},
            error => {
                if (error.status==200){
                    this.successfullEditAvailability.emit();
                    this.toastr.success(
                        this.translate.instant('Availability.successfully.edit'), '',
                        {positionClass: 'toast-bottom-right'}
                    );
                }
                else if (this.model.status != 1){
                    this.toastr.warning(
                        this.translate.instant('Availability.notSuccessfull.edit'), '',
                        {positionClass: 'toast-bottom-right'}
                    );
                }
            });           
    }

    removeAvailability() {
        this.model.status = 1;
        this.toastr.warning(
            this.translate.instant('Availability.removed'), '',
            {positionClass: 'toast-bottom-right'}
        );
        return this.editAvailability();
        
    }
}