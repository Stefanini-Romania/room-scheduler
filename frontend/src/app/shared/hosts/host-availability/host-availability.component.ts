import {Component, NgModule, Input, EventEmitter} from '@angular/core'
import {BrowserModule} from '@angular/platform-browser'
import {TranslateService, LangChangeEvent} from "@ngx-translate/core";
import {NgbModal, NgbModalRef,NgbPaginationConfig, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import {User} from '../../../shared/models/user.model';
import {HostService} from './../../services/host.service';
import {Availability} from './../../models/availability.model';
import {HostSelector} from './../host-selector/host-selector.component';
import {HostAvailabilityForm} from './../host-availability-form/host-availability-form.component';


@Component({
    selector: 'host-availability',
    templateUrl: './host-availability.component.html',
    providers: []
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
  
    public startDate: Date;
    public endDate: Date;  
    public hostId: number;
    public exception: boolean;
    public displayDate = new Date();
    
    constructor(public hostService: HostService, translate: TranslateService, private modalService: NgbModal) {
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
            this.model.endDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate()+4);
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
            this.model.endDate = new Date(newDate.getFullYear(), newDate.getMonth(), newDate.getDate()+4);
        }
        this.availabilities = [];
        this.exceptions = [];
        this.events = [];
       
        this.hostService.HostAvailabilityList(
            newDate,
            this.selectedHost.id, 
            this.model.endDate, 
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
}