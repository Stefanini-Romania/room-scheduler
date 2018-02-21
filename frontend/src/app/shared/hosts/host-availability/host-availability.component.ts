import {Component, NgModule, Input, EventEmitter, AfterViewInit} from '@angular/core'
import {BrowserModule} from '@angular/platform-browser'
import {TranslateService, LangChangeEvent} from "@ngx-translate/core";
import {NgbModal, NgbModalRef,NgbPaginationConfig, ModalDismissReasons, NgbDatepickerConfig, NgbDateStruct} from '@ng-bootstrap/ng-bootstrap';

import {User} from '../../../shared/models/user.model';
import {HostService} from './../../services/host.service';
import {Availability} from './../../models/availability.model';
import {HostSelector} from './../host-selector/host-selector.component';
import {HostAvailabilityForm} from './../host-availability-form/host-availability-form.component';
import {HostExceptionForm} from './../host-exception-form/host-exception-form.component';
import { AuthService } from '../../../auth/shared/auth.service';
import {RoleEnum} from '../../models/role.model';


@Component({
    selector: 'host-availability',
    templateUrl: './host-availability.component.html',
    providers: [NgbDatepickerConfig]
})

export class HostAvailability{
    @Input() host: User;
   
   
    public availabilities: Availability[] = [];
    public exceptions: Availability[] = [];
    public events: Availability[] = [];
    public model: Availability = <Availability>{};
    public selectedHost: User;
    public roomId: number;
    users: User[] = [];
    hosts: any[] = [];
    availabilityHostGroupName: string;
    day: any[] = [];
    public availPage;
    public execPage;
    public isHost: boolean;
  
  
    public startDate: Date;
    public endDate: Date;  
    public hostId: number;
    public exception: boolean;
    public displayDate = new Date();
    
    constructor(public hostService: HostService, translate: TranslateService, private modalService: NgbModal, private datePickerConfig: NgbDatepickerConfig, private authService: AuthService) {
        datePickerConfig.markDisabled = (date: NgbDateStruct) => {
            const day = new Date(date.year, date.month - 1, date.day);
            return day.getDay() === 0 || day.getDay() === 6;
          };

          if(this.authService.getLoggedUser().hasRole(RoleEnum.host)){
              this.isHost=true;
          }

          if(this.isHost==true){
              this.selectedHost = this.authService.getLoggedUser();
              this.listAvailabilities();
          }
          
    }

    onHostChanged(selectedHost: User) {
        this.selectedHost = selectedHost;
        this.listAvailabilities();
        
    }

    listAvailabilities() {
        // if (!this.startDate || !this.endDate || !this.selectedHost) {
        //     return;
        // }
       
        if (this.model.startDate){
            JSON.stringify(this.model.startDate);
            var newDate = new Date(this.model.startDate["year"], this.model.startDate["month"]-1, this.model.startDate["day"], 0);
            JSON.stringify(newDate);
            let dayOfWeek = newDate.getDay();
            let currentDate = newDate.getDate();
            while(dayOfWeek!==1){
                dayOfWeek--;
                currentDate=currentDate-1;
            }
            newDate.setDate(currentDate);
        } 
        else {
            newDate = this.displayDate;
            let dayOfWeek = newDate.getDay();
            let currentDate = newDate.getDate();
            while(dayOfWeek!==1){
                dayOfWeek--;
                currentDate=currentDate-1;
            }
            newDate.setDate(currentDate);
        }
        this.availabilities = [];
        this.exceptions = [];
        this.events = [];
       
        this.hostService.HostAvailabilityList(
            newDate,
            this.selectedHost.id,  
            this.model.roomId).subscribe((events: Availability[]) => {
                for (let day of events) {    
                    if (day.availabilityType == 2) {
                        this.exceptions.push(<Availability>day);
                    } else {
                        this.availabilities.push(<Availability>day);
                    }                           
                }
        });        
    } 

    onSelectAvailability(model: Availability) {
        const modalRef: NgbModalRef = this.modalService.open(HostAvailabilityForm); 
        modalRef.componentInstance.model = model; 
        modalRef.componentInstance.host = this.selectedHost;
        modalRef.componentInstance.successfullEditAvailability.subscribe(() => {
            modalRef.close();     
            this.listAvailabilities();
        });   
    }

    onSelectException(model: Availability) {
        const modalRef: NgbModalRef = this.modalService.open(HostExceptionForm); 
        modalRef.componentInstance.model = model; 
        modalRef.componentInstance.host = this.selectedHost;
        modalRef.componentInstance.successfullEditException.subscribe(() => {
            modalRef.close();     
            this.listAvailabilities();
        });   
    }

    onAddAvailability() {
        const modalRef:NgbModalRef = this.modalService.open(HostAvailabilityForm, {});
        modalRef.componentInstance.host = this.selectedHost;
        modalRef.componentInstance.successfullAddAvailability.subscribe(() => {
            this.listAvailabilities();
            modalRef.close();
    })}

    onAddException() {
        const modalRef:NgbModalRef = this.modalService.open(HostExceptionForm, {});
        modalRef.componentInstance.host = this.selectedHost;
        modalRef.componentInstance.successfullAddException.subscribe(() => {
            modalRef.close();
            this.listAvailabilities();
        })
    }
}