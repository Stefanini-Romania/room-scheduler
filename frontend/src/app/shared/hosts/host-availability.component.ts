import {Component, NgModule} from '@angular/core'
import {BrowserModule} from '@angular/platform-browser'
import {TranslateService} from "@ngx-translate/core";

import {User} from '../../shared/models/user.model';
import {Event} from '../../shared/models/event.model';
import {EventService} from './../../calendars/shared/event.service';
import {EventTypeEnum} from '../../shared/models/event.model';
import {EventStatusEnum} from '../../shared/models/event.model';
import {HostAvailabilityService} from './../services/host-availability.service';

@Component({
    selector: 'host-availability',
    templateUrl: './host-availability.component.html',
    providers: [EventService, HostAvailabilityService]
})

export class HostAvailability{

    events: Event[] = [];
    users: User[] = [];
    availabilities: Event[] = [];
    hosts: any[] = [];
    availabilityHostGroupName: string;
    host: User[] = [];

    
    public date: Date = new jqx.date();
    public startDate: Date;
    public endDate: Date;
    
    public hostId: number;
    private previousValues: any;

    constructor(public HostAvailabilityService: HostAvailabilityService) {

    }
    ngAfterViewInit(){
        this.availabilities = [];
        this.HostAvailabilityService.HostAvailabilityList().subscribe((availabilities: any) => {
            for (let day of availabilities) {
                console.log(availabilities);
                //for (let event of availabilities[host][day])
                this.availabilities.push(<Event>availabilities);
            }
        });
    }
}