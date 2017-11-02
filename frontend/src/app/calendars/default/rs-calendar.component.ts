import { EventEmitter } from 'events';
import { Component, ViewChild, AfterViewInit } from '@angular/core';
import {jqxSchedulerComponent} from '../../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import { RoomSelector } from './../../rooms/roomSelector.component';

import {EventService} from '../shared/event.service'
@Component({
  selector: 'rs-calendar-component',
  templateUrl: './rs-calendar.component.html',
  providers: [EventService, RoomSelector]
})

export class RSCalendarComponent {
    @ViewChild('schedulerReference') scheduler: jqxSchedulerComponent;
    private events: any;
    public startDate: Date;
    public roomId:number;

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
    date: Date;

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

  

    constructor(private _eventService: EventService) {}

    today() {
        
        this.date = new Date();
    }

    setView(view: string) {
        this.view = view;
        this.scheduler.view(view);
    }

    goBack() {
        console.log(this.scheduler.view());
        const days = this.scheduler.view() == 'weekView' ? 7 : 1;
        this.date = this.scheduler.date().addDays(-days);
    }

    goForward() {
        console.log(this.scheduler.view());
        const days = this.scheduler.view() == 'weekView' ? 7 : 1;
        this.date = this.scheduler.date().addDays(days);
    }

    test () {
        console.log(this.scheduler.view());
        
    }

    
    calendarUpdate() {
        // @TODO currentCalendarDate - get crrent calendar date
        // @TODO roomId - get the roomId from the room-selector
        // @TODO this.renderCalendar(currentCalendarDate, roomId);
    }

    ngAfterViewInit(): void {
        this.scheduler.ensureAppointmentVisible('id1');

        this.renderCalendar(new Date());
    }


    private renderCalendar(startDate: Date, roomId: number = null) {
        this.events = [];
        const events = this._eventService.listEvents(startDate, new Date(startDate.getDate() + 10) , roomId).subscribe((events: Event[]) => {
            for (let event of events) {
                this.events.push({
                    id: event['id'],
                    // description: "Some descript",
                    location: "",
                    subject: "Masaj",
                    calendar: "Room " + event['id'],
                   start: new Date(event['startDate']),
                   end: new Date(event['endDate']),
                });
            };
            this.refreshdata();
      
        });    
    }

    refreshdata(){
        this.source.localData = this.events;
        this.dataAdapter = new jqx.dataAdapter(this.source);
    }


}

