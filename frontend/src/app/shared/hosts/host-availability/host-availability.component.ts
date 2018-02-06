import {Component, NgModule, Host, EventEmitter} from '@angular/core'
import {BrowserModule} from '@angular/platform-browser'
import {TranslateService, LangChangeEvent} from "@ngx-translate/core";

import {User} from '../../../shared/models/user.model';
import {HostService} from './../../services/host.service';
import {Availability} from './../../models/availability.model';
import {HostSelector} from './../host-selector/host-selector.component';


@Component({
    selector: 'host-availability',
    templateUrl: './host-availability.component.html',
    providers: [HostService]
})

export class HostAvailability{
   
    public availabilities: Availability[] = [];
    public exceptions: Availability[] = [];
    public events: Availability[] = [];
    public model: Availability = <Availability>{};
    public selectedHost: User;
    public roomId: number;
    users: User[] = [];
    hosts: any[] = [];
    availabilityHostGroupName: string;
    host: User[] = [];
    day: any[] = [];
  
    public startDate: Date;
    public endDate: Date;  
    public hostId: number;
    public exception: boolean;
  
    constructor(public HostService: HostService, translate: TranslateService) {
        
    }

    onHostChanged(selectedHost: User) {
        this.selectedHost = selectedHost;
        this.listAvailabilities();
    }

    listAvailabilities() {
        // if (!this.startDate || !this.endDate || !this.selectedHost) {
        //     return;
        // }
       
        this.getStartOfWeek();
        this.startDate = new Date();
        this.availabilities = [];
        this.exceptions = [];
        this.events = [];
        
        this.HostService.HostAvailabilityList(this.model.startDate, this.selectedHost.id, this.model.endDate, this.model.roomId).subscribe((events: Availability[]) => {
            for (let day of events) {    
                if (day.availabilityType == 2) {
                    this.exceptions.push(<Availability>day);
                } else {
                    this.availabilities.push(<Availability>day);
                }                           
            }
        });       
    }   

    getStartOfWeek(){
        this.model.startDate = new Date();
        let dayOfWeek = this.model.startDate.getDay();
        let currentDate = this.model.startDate.getDate();
        while(dayOfWeek!==1){
            dayOfWeek--;
            currentDate=currentDate-1;
        }
        this.model.startDate.setDate(currentDate);
        this.model.endDate = new Date(this.model.startDate.getFullYear(), this.model.startDate.getMonth(), this.model.startDate.getDate()+4);
    } 
}