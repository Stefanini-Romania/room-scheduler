import {Component, Input, NgModule, OnInit, Output, EventEmitter} from '@angular/core'
import {TranslateService} from "@ngx-translate/core";
import {NgbActiveModal, NgbModalRef, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ToastrService} from 'ngx-toastr';
import {FormBuilder, FormGroup} from '@angular/forms';
import { SelectControlValueAccessor } from '@angular/forms';

import {User} from '../../models/user.model';
import {Availability} from '../../models/availability.model';
import {HostService} from './../../services/host.service';
import {Room} from '../../models/room.model';


@Component({
    selector: 'host-availability-form',
    templateUrl: './host-availability-form.component.html',
    providers: []
})

export class HostAvailabilityForm{
    @Input() host: User;

    @Output()
    successfullAddAvailability = new EventEmitter;

    public model: Availability = <Availability>{};
    public startHour;
    public endHour;
    public selectedRoom: Room;
    public selectedHost: User;
    public addAvail: boolean;
    public minuteStep = 30;
    
    constructor( private formBuilder: FormBuilder, private translate: TranslateService, public activeModal: NgbActiveModal, private hostService: HostService, private toastr: ToastrService){
    }
 
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


    }