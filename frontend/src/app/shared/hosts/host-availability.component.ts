import {Component, NgModule} from '@angular/core'
import {BrowserModule} from '@angular/platform-browser'
import {TranslateService} from "@ngx-translate/core";

import {User} from '../../shared/models/user.model';
import {Event} from '../../shared/models/event.model';
import {EventService} from './../../calendars/shared/event.service';
import {EventTypeEnum} from '../../shared/models/event.model';
import {EventStatusEnum} from '../../shared/models/event.model';
import {HostAvailabilityService} from './../services/host-availability.service';
import {Availability} from './../models/availability.model';
//import {HostService} from './../services/host-availability.service';

@Component({
    selector: 'host-availability',
    templateUrl: './host-availability.component.html',
    providers: [EventService, HostAvailabilityService]
})

export class HostAvailability{

    users: User[] = [];
    public availabilities: Availability[] = [];
    public model: Availability = <Availability>{};
    public selectedHost: User;
    public roomId: number;
    hosts: any[] = [];
    availabilityHostGroupName: string;
    host: User[] = [];
    day: any[] = [];
  
    public startDate: Date;
    public endDate: Date;  
    public hostId: number;

    constructor(public HostAvailabilityService: HostAvailabilityService) {}
    
    ngAfterViewInit(){
        this.listAvailabilities();
    }

        listAvailabilities()
    {
        // if (!this.startDate || !this.endDate || !this.selectedHost) {
        //     return;
        // }
        this.getStartOfWeek();
        this.startDate = new Date();
        
        let hosts = {};

        this.availabilities = [];
        this.HostAvailabilityService.HostAvailabilityList(this.model.startDate, this.hostId = 3, this.model.endDate, this.roomId).subscribe((availabilities: Availability[]) => {
            for (let day of availabilities) {           
                // if (!hosts[day.hostId]) {
                //     hosts[day.hostId] = [];
                // }
                // hosts[availabilities.].push(availabilities);
                //for (let event of availabilities[host][day])
                this.availabilities.push(<Availability>day);
            }
            console.log("avails", availabilities);
            console.log("days", this.day);
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