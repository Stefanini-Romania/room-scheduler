import {Component, Input, NgModule, OnInit, Output, EventEmitter} from '@angular/core'
import {TranslateService} from "@ngx-translate/core";
import {NgbActiveModal, NgbModalRef, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ToastrService} from 'ngx-toastr';
import {FormBuilder, FormGroup, FormControl} from '@angular/forms';
import {SelectControlValueAccessor} from '@angular/forms';
import {NgbDatepickerConfig, NgbDateStruct, NgbTimepickerConfig, NgbTimeStruct} from '@ng-bootstrap/ng-bootstrap';

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
    @Input() 
    host: User;

    @Output()
    successfullAddAvailability = new EventEmitter;
    successfullEditAvailability = new EventEmitter;

    public model: Availability = <Availability>{};
    public selectedRoom: Room;
    public selectedHost: User;
    public addAvail: boolean;
    public minuteStep = 30;
    public title: string;
    public displayDate = new Date();
    public errorMessages: any = {}; 
    public availabilityStartDate; 
    public availabilityEndDate;
    public dayOfWeek;
    public startHour: NgbTimeStruct;
    public endHour: NgbTimeStruct;
    public seconds = false;

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
   

    end = new FormControl('', (control: FormControl) => {
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

    ngOnInit() {
        this.title = this.model.id ? 'Availability.edit': 'Availability.add';
        // if (this.model.id) {
        //     this.model.startDate = new Date();
        //     this.displayDate = new Date(this.model.startDate.getDate())
        // }
        // else {
        //     this.displayDate = new Date();
        //     console.log(this.displayDate);
        // }
        if (this.model.id) {
            let startH = (new Date(this.model.startDate)).getHours();
            let startM = (new Date(this.model.startDate)).getMinutes();
            let endH = (new Date(this.model.endDate)).getHours();
            let endM = (new Date(this.model.endDate)).getMinutes();
            this.startHour = {hour: startH, minute: startM, second: 0};
            this.endHour = {hour: endH, minute: endM, second: 0};
        } 
    }

    onRoomChanged(selectedRoom: Room) {
        this.selectedRoom = selectedRoom;
    }

    dateFormat(Availability) {
        if (this.model.id) {   
            this.availabilityStartDate = new Date(this.model.startDate);
            this.availabilityStartDate.setHours(this.startHour.hour);
            JSON.stringify(this.availabilityStartDate);    

            this.model.endDate = new Date(this.availabilityStartDate.getFullYear(), this.availabilityStartDate.getMonth(),this.availabilityStartDate.getDate(), this.endHour["hour"], this.endHour["minute"]) ;
            this.availabilityEndDate = this.model.endDate;
            JSON.stringify(this.availabilityEndDate);
        }
        else {
            this.availabilityStartDate = new Date(this.model.startDate["year"], this.model.startDate["month"]-1, this.model.startDate["day"], this.startHour["hour"], this.startHour["minute"], 0);
            JSON.stringify(this.availabilityStartDate);
            this.dayOfWeek = this.availabilityStartDate.getDay();
            let currentDate = this.availabilityStartDate.getDate();
            while(this.dayOfWeek!==1){
                this.dayOfWeek--;
                currentDate=currentDate-1;
            }
            this.availabilityStartDate.setDate(currentDate);
        
            this.model.endDate = new Date(this.availabilityStartDate.getFullYear(), this.availabilityStartDate.getMonth(),this.availabilityStartDate.getDate(),  this.endHour["hour"], this.endHour["minute"]) ;
            this.availabilityEndDate = this.model.endDate;
        }    
    
    }

    addAvailability(){
        if(this.model.startDate && this.startHour && this.endHour){
            this.dateFormat(Availability);
            this.model.daysOfWeek = this.selectedDaysOfWeek;
            this.model.occurrence=this.selectedOccurrence.value;
            this.hostService.AddHostAvailability( 
                this.availabilityStartDate,
                this.availabilityEndDate, 
                this.model.availabilityType = 0, 
                this.model.daysOfWeek, 
                this.model.occurrence, 
                this.selectedRoom.id,
                this.host.id).subscribe(() => 
                {},
                error => {
                    if(error.status==200){
                        this.successfullAddAvailability.emit();
                        this.toastr.success(
                            this.translate.instant('Availability.successfully.added'), '',
                            {positionClass: 'toast-bottom-right'}
                        );
                    }
    
                    else{
                        this.errorMessages = {'generic': [error.error.message]};
                        for (let e of error.error.errors) {
                            let field = 'generic';
                            
                            if (['StartDate', 'EndDate'].indexOf(e.field) >= 0) {
                                field = e.field;
                            }  
                            if (!this.errorMessages[field]) {
                                this.errorMessages[field] = [];
                            }      
                            this.errorMessages[field].push(e.errorCode);
                        }     
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

        this.dateFormat(Availability);  
        this.model.occurrence = this.selectedOccurrence.value;
        this.model.roomId = this.selectedRoom.id;

        this.hostService.EditHostAvailability(
            this.model.id, 
            this.availabilityStartDate,
            this.availabilityEndDate,
            this.model.status, 
            this.model.occurrence, 
            this.model.roomId).subscribe(
                () => {},
                error => {
                    if (error.status == 200 && this.model.status !== 1){
                        this.successfullEditAvailability.emit();
                        this.toastr.success(
                            this.translate.instant('Availability.successfully.edit'), '',
                            {positionClass: 'toast-bottom-right'}
                        );
                    }
                    else if (this.model.status !== 1){
                        this.errorMessages = {'generic': [error.error.message]};
                        for (let e of error.error.errors) {
                            let field = 'generic';
                            
                            if (['StartDate', 'EndDate'].indexOf(e.field) >= 0) {
                                field = e.field;
                            }  
                            if (!this.errorMessages[field]) {
                                this.errorMessages[field] = [];
                            }      
                            this.errorMessages[field].push(e.errorCode);
                        }     
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
        this.activeModal.close();
        return this.editAvailability();   
    }
}