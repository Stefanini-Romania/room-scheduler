import {Component, NgModule, OnInit} from '@angular/core'
import {TranslateService} from "@ngx-translate/core";
import {NgbActiveModal, NgbModalRef, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {ToastrService} from 'ngx-toastr';
import {FormBuilder, FormGroup} from '@angular/forms';

import {User} from '../../models/user.model';
import {Availability} from '../../models/availability.model';
import {HostService} from './../../services/host.service';
import {Room} from '../../models/room.model';

@Component({
    selector: 'host-availability-form',
    templateUrl: './host-availability-form.component.html',
    providers: [HostService]
})

export class HostAvailabilityForm{
    public model: Availability = <Availability>{};
    public startHour;
    public endHour;
    public selectedRoom: Room;

    constructor( private formBuilder: FormBuilder, private translate: TranslateService, public activeModal: NgbActiveModal, private hostService: HostService, private toastr: ToastrService){}

    ngOnInit() {
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
      onRoomChanged(selectedRoom: Room) {
        this.selectedRoom = selectedRoom;
    }

      addAvailability(){
         this.model.daysOfWeek = this.selectedDaysOfWeek;
          this.hostService.AddHostAvailability(
              this.model.startDate, 
              this.model.endDate, 
              this.model.availabilityType = 0, 
              this.model.daysOfWeek, 
              this.model.occurrence, 
              this.selectedRoom.id).subscribe(() => {
               
                this.toastr.success(
                    this.translate.instant('rooms.created'), '',
                    {positionClass: 'toast-bottom-right'}
                );                                                         
            },
            error => {
                
            });    

      }


    }