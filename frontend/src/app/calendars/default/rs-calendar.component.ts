import {EventEmitter} from 'events';
import {Component, ViewChild} from '@angular/core';
import {jqxSchedulerComponent} from '../../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import {EventService} from '../shared/event.service';
import {RoomSelector} from '../../rooms/room-selector/room-selector.component';
import {Room} from '../../shared/models/room.model';
import {Event} from '../../shared/models/event.model';


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
    public selectedStartDate: Date;
    public selectedEndDate: Date;
    public roomId: number;
    public hostId: number;

    source: any =
        {
            dataType: "array",
            dataFields: [
                {name: 'id', type: 'string'},
                {name: 'description', type: 'string'},
                {name: 'location', type: 'string'},
                {name: 'subject', type: 'string'},
                {name: 'calendar', type: 'string'},
                {name: 'start', type: 'date'},
                {name: 'end', type: 'date'}
            ],
            id: 'id',
            localData: []
        };

    dataAdapter: any = new jqx.dataAdapter(this.source);

    resources: any = {
        colorScheme: "scheme05",
        dataField: "calendar",
        source: new jqx.dataAdapter(this.source)
    };

    view = 'weekView';

    views: any[] = [
        {type: 'dayView', showWeekends: false, timeRuler: {scaleStartHour: 9, scaleEndHour: 18}},
        {type: 'weekView', showWeekends: false, timeRuler: {scaleStartHour: 9, scaleEndHour: 18}},
    ];


    constructor(private eventService: EventService) {
    }

    showEditDialog() {
        let date = this.scheduler.getSelection();
        this.selectedStartDate = new Date(date.from.toDate());
        this.selectedEndDate = new Date(date.to.toDate());

    }

    isView(view: string): boolean {
        return view == this.scheduler.view();
    }
    setView(view: string) {
        this.view = view;
        this.scheduler.view(view);
    }

    goToToday() {
        this.startDate = new Date();
        this.renderCalendar(this.startDate, this.roomId);
    }

    goBack() {
        const days = this.isView('weekView') ? 7 : 1;
        this.startDate = new Date(this.scheduler.date().addDays(-days));
        this.renderCalendar(this.startDate, this.roomId);
    }

    goForward() {
        const days = this.isView('weekView') ? 7 : 1;
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
        this.eventService.listEvents(startDate, endDate, roomId, this.hostId).subscribe((events: Event[]) => {
            for (let event of events) {
                this.events.push(<Event>event);
            }
            this.refreshCalendar();
        });
    }

    createEvent() {
        console.log(this.model);
        this.eventService.createEvent(this.model.startDate = this.selectedStartDate, this.model.endDate = this.selectedEndDate, this.model.eventType = 0, this.model.roomId = 1, this.model.hostId = 3, this.model.attendeeId = 1, this.model.eventStatus = 4, this.model.notes).subscribe(
            () => {
                this.refreshCalendar();
            },
            error => {
                this.errorMessage = "Unable to create event";
            })
    }

    refreshCalendar() {
        this.source.localData = this.events;
        this.dataAdapter = new jqx.dataAdapter(this.source);
    }

}

