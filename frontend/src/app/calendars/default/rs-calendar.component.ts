import {Component, ViewChild} from '@angular/core';
import {jqxSchedulerComponent} from '../../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import {EventService} from '../shared/event.service';
import {RoomSelector} from '../../rooms/room-selector/room-selector.component';
import {Room} from '../../shared/models/room.model';
import {Event} from '../../shared/models/event.model';

//cb360a413af57cb163691a7fee3409e860cfe85a

@Component({
    selector: 'rs-calendar-component',
    templateUrl: './rs-calendar.component.html',
    providers: [EventService, RoomSelector]
})

export class RSCalendarComponent {
    @ViewChild('schedulerReference') scheduler: jqxSchedulerComponent;

    events: Event[];
    model: Event = <Event> {};
    createErrorMessages: any = {};

    public startDate: Date;
    public selectedStartDate: Date;
    public selectedEndDate: Date;
    public roomId: number;
    public hostId: number;
    public eventId: number;
    public saveEventTitle: string;

    public view = 'weekView';

    closeResult: string;

    source: any = {
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

    eventDataFields: any = {
        from: "start",
        to: "end",
        id: "id",
        description: "description",
        location: "location",
        subject: "subject",
        resourceId: "calendar"
    };

    dataAdapter: any = new jqx.dataAdapter(this.source);

    resources: any = {
        colorScheme: "scheme05",
        dataField: "calendar",
        source: null
    };

    views: any[] = [
        {type: 'dayView', showWeekends: false, timeRuler: {scaleStartHour: 9, scaleEndHour: 18}},
        {type: 'weekView', showWeekends: false, timeRuler: {scaleStartHour: 9, scaleEndHour: 18}},
    ];

    constructor(private eventService: EventService, private modalService: NgbModal) {
    }

    /*
    refreshCalendar() {
        let events = [];
        for (let event of this.events) {
            const e: any = Object.assign({}, event);
            e.subject = "Quarterly Project Review Meeting";
            e.calendar = "Room " + event.roomId;
            events.push(e);
        }
        this.source.localData = events;
        this.dataAdapter = new jqx.dataAdapter(this.source);
    }
    transformEventToAppointment(event: Event) {
        let appointment: any = Object.assign({}, event);
        appointment.subject = "Quarterly Project Review Meeting";
        appointment.calendar = "Room " + event.roomId;
        appointment.startDate = new Date(event.startDate);
        appointment.endDate = new Date(event.endDate);

        return appointment;
    }
     */
    refreshCalendar() {
        let events = [];
        for (let event of this.events) {
            events.push(<any>{
                id: event.id,
                description: event.notes,
                location: "",
                subject: "Massage",
                calendar: "Room " + event.roomId,
                start: new Date(event.startDate),
                end: new Date(event.endDate)
            });
        }
        this.source.localData = events;
        this.dataAdapter = new jqx.dataAdapter(this.source);
    }

    private getDismissReason(reason: any): string {
        if (reason === ModalDismissReasons.ESC) {
            return 'by pressing ESC';
        } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
            return 'by clicking on a backdrop';
        } else {
            return `with: ${reason}`;
        }
    }

    isView(view: string): boolean {
        return view == this.view;
    }

    setView(view: string) {
        this.view = view;
        this.scheduler.view(this.view);
    }

    goToToday() {
        this.startDate = new Date();
        this.renderCalendar();
    }

    goBack() {
        const days = this.isView('weekView') ? 7 : 1;
        this.startDate = new Date(this.scheduler.date().addDays(-days));
        this.renderCalendar();
    }

    goForward() {
        const days = this.isView('weekView') ? 7 : 1;
        this.startDate = new Date(this.scheduler.date().addDays(days));
        this.renderCalendar();
    }

    onRoomChanged(selectedRoom: Room) {
        this.roomId = selectedRoom.id;
        this.renderCalendar();
    }

    onRoomsLoaded(rooms: Room[]) {
        // select 1st room when rooms are loaded
        if (rooms.length) {
            this.roomId = rooms[0].id;
            this.renderCalendar();
        }
    }

    ngAfterViewInit(): void {
        this.scheduler.ensureAppointmentVisible('id1');

        this.startDate = new Date();
        this.renderCalendar();
    }

    private renderCalendar() {
        this.events = [];
        let endDate = new Date();
        endDate.setDate(this.startDate.getDate() + 7);
        this.eventService.listEvents(this.startDate, endDate, this.roomId, this.hostId).subscribe((events: Event[]) => {

            for (let event of events) {
                this.events.push(<Event>event);
            }

            this.refreshCalendar();
        });
    }

    showEditDialog(content) {
        // @TODO detect create or edit
        this.saveEventTitle = 'calendar.event.create';
        this.model = new Event();
        this.model.startDate = this.selectedStartDate;
        this.model.endDate = this.selectedEndDate;
        this.model.eventType = 0; // @TODO use constants
        this.model.eventStatus = 4; // @TODO use constants
        this.model.roomId = this.roomId;
        // this.model.hostId = 3; WE SHOULD NOT NEED A HOST
        this.model.attendeeId = 1; // @TODO get user id from logged user

        //this.saveEventTitle = 'calendar.event.edit';
        //this.model = // @TODO get event from the selected event (use this.events[eventId]) where we have all the events;

        this.createErrorMessages = [];
        let date = this.scheduler.getSelection();
        this.selectedStartDate = new Date(date.from.toDate());
        this.selectedEndDate = new Date(date.to.toDate());

        this.modalService.open(content).result.then((result) => {
            this.closeResult = `Closed with: ${result}`;
        }, (reason) => {
            this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
        });
    }

    saveEvent() {
        // clear any previous errors
        this.createErrorMessages = {};

        // try to save
        this.eventService.save(this.model).subscribe(
            () => {
                this.renderCalendar();
            },
            error => {
                this.createErrorMessages = {'generic': [error.error.message]};

                // build error message
                for (let e of error.error.errors) {
                    let field = 'generic';
                    if (['StartDate', 'EndDate'].indexOf(e.field) >= 0) {
                        field = e.field;
                    }

                    if (!this.createErrorMessages[field]) {
                        this.createErrorMessages[field] = [];
                    }

                    this.createErrorMessages[field].push(e.errorCode);
                }

                this.renderCalendar();
            });
    }
}