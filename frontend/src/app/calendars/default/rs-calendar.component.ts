
import { NumberFormatStyle } from '@angular/common/src/pipes/intl';
import { EventEmitter } from 'events';
import { Component, ViewChild, AfterViewInit } from '@angular/core';
import {jqxSchedulerComponent} from '../../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import { RoomSelector } from './../../rooms/roomSelector.component';
import {Room} from './../../rooms/room.model';
import { Observable } from 'rxjs/Observable';
import { Event } from '../../shared/event.model';
import {EventService} from '../shared/event.service';
import { Router, RouterModule } from '@angular/router';

@Component({
  selector: 'rs-calendar-component',
  templateUrl: './rs-calendar.component.html',
  providers: [EventService, RoomSelector]
})

export class RSCalendarComponent {
    @ViewChild('schedulerReference') scheduler: jqxSchedulerComponent;
    events: Event[];
    model: Event = <Event> {};
   
    public errorMessage: string = '';
   
    public startDate: Date;
    public roomId:number;
    public hostId: number;

    source: any =
    {
        dataType: "array",
        dataFields: [
            { name: 'id', type: 'string' },
            { name: 'description', type: 'string' },
            { name: 'location', type: 'string' },
            { name: 'subject', type: 'string' },
            { name: 'calendar', type: 'string' },
            { name: 'start', type: 'date' },
            { name: 'end', type: 'date' }
        ],
        id: 'id',
        localData: []
    };

    dataAdapter: any = new jqx.dataAdapter(this.source);

    appointmentDataFields: any =
    {
        from: "start",
        to: "end",
        id: "id",
        description: "description",
        location: "location",
        subject: "subject",
        resourceId: "calendar"
    };

    resources: any =
    {
        colorScheme: "scheme05",
        dataField: "calendar",
        source: new jqx.dataAdapter(this.source)
    };

    view = 'weekView';

    views: any[] =
    [
        
        { type: 'dayView', showWeekends: false, timeRuler: { scaleStartHour: 9, scaleEndHour: 18 } },
        { type: 'weekView', showWeekends: false, timeRuler: { scaleStartHour: 9, scaleEndHour: 18 }},
      //   { type: 'monthView', showWeekends: false }
    ];  


    constructor(private eventService: EventService) {}

    today() {
        this.startDate = new Date();
        this.renderCalendar(this.startDate, this.roomId);
    }

    setView(view: string) {
        this.view = view;
        this.scheduler.view(view);
    }

    goBack() {
        const days = this.scheduler.view() == 'weekView' ? 7 : 1;
        this.startDate = new Date(this.scheduler.date().addDays(-days));
        this.renderCalendar(this.startDate, this.roomId);
    }

    goForward() {
        const days = this.scheduler.view() == 'weekView' ? 7 : 1;
        this.startDate = new Date(this.scheduler.date().addDays(days));
        this.renderCalendar(this.startDate, this.roomId);
    }

   calendarUpdate(selectedRoom: Room) {
        this.roomId = selectedRoom.id;
        this.renderCalendar(this.startDate, this.roomId);
    }

    ngAfterViewInit(): void {
        this.scheduler.ensureAppointmentVisible('id1');

        this.startDate = new Date();
        this.renderCalendar(this.startDate, this.roomId);
    }


    private renderCalendar(startDate: Date, roomId: number) {
        this.events = [];
        let endDate = new Date();
        endDate.setDate(startDate.getDate() + 7);
        const events = this.eventService.listEvents(startDate , endDate, roomId, this.hostId).subscribe((events: Event[]) => {
            for (let event of events) {
                this.events.push(<Event>event);
            };
            this.refreshdata();
            // console.log(this.events);
      
        });    
    }

    newEvent(){
        this.startDate =  new Date();
        let endDate = new Date();
        endDate.setMinutes(endDate.getMinutes()+30);
        this.eventService.createEvents(this.model.startDate = this.startDate, this.model.endDate = endDate, this.model.eventType = 0, this.model.roomId = 1, this.model.hostId = 3, this.model.attendeeId = 1, this.model.eventStatus = 4).subscribe(
            data =>{
                this.refreshdata();
            },
            error => {
                this.errorMessage = "Unable to create event";
            })
    }

    refreshdata(){
        this.source.localData = this.events;
        this.dataAdapter = new jqx.dataAdapter(this.source);
    }


}

