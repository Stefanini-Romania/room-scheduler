import {Router} from '@angular/router';
import {Component, ViewChild, AfterViewInit} from '@angular/core';
import {jqxSchedulerComponent} from '../../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import {NgbModal, ModalDismissReasons, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';
import {EventService} from '../shared/event.service';
import {RoomSelector} from '../../rooms/room-selector/room-selector.component';
import {Room} from '../../shared/models/room.model';
import {Event} from '../../shared/models/event.model';
import {AuthService} from '../../auth/shared/auth.service';
import {EventTypeEnum} from '../../shared/models/event.model';
import {EventStatusEnum} from '../../shared/models/event.model';

declare var $ :any;

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

    public xstartDate: Date;
    public xendDate: Date;

    public startDate: Date = new $.jqx.date();
    public endDate: Date;
    public roomId: number;
    public hostId: number;

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
        resourceId: "calendar",
        timeZone: "UTC"
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

    localization = {
        // separator of parts of a date (e.g. '/' in 11/05/1955)
        '/': '/',

        // separator of parts of a time (e.g. ':' in 05:44 PM)
        ':': ':',

        // the first day of the week (0 = Sunday, 1 = Monday, etc)
        firstDay: 0,
        days: {
            // full day names
            names: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"],
            // abbreviated day names
            namesAbbr: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
            // shortest day names
            namesShort: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"]
        },
        months: {
            // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
            names: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""],
            // abbreviated month names
            namesAbbr: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""]
        },

        contextMenuEditAppointmentString: "Edit Appointment",
        contextMenuCreateAppointmentString: "Create New Appointment",
    };

    constructor(private router: Router, private eventService: EventService, private modalService: NgbModal, private authService: AuthService) {
    }

    private modalRef: NgbModalRef;
    private previousValues: any;

    refreshCalendar() {
        let events = [];
        for (let event of this.events) {
            events.push(<any>{
                id: event.id,
                description: event.notes,
                location: "",
                subject: "Massage " + event.eventType,
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
        this.startDate = new $.jqx.date();
        this.renderCalendar();
    }

    showCalendarsDate($event) {
        if (!this.previousValues || ($event.args.from.toString() !== this.previousValues.from.toString())) {
            this.previousValues = $event.args;

            let x: Date;

            x = $event.args.from.toDate();
            this.xstartDate = new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate() + (this.isView('weekView') ? 1 : 0),
                0, 0, 0
            ));

            //x = $event.args.to.toDate();
            this.xendDate = new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate() + (this.isView('weekView') ? 4 : -1),
                23, 59, 59
            ));
console.log("HERE1", this.xstartDate, this.xendDate);
            this.renderCalendar();
        }
    }


    goBack() {
        const days = this.isView('weekView') ? 7 : 1;
        this.startDate = new Date(this.scheduler.date().addDays(-days).toString());
    }

    goForward() {
        const days = this.isView('weekView') ? 7 : 1;
        this.startDate = new Date(this.scheduler.date().addDays(days).toString());
    }

    onRoomChanged(selectedRoom: Room) {
        this.startDate = new Date(this.scheduler.date().toString());
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

    private renderCalendar() {
        if (!this.xstartDate || !this.xendDate) {
            return;
        }

        console.log("HERE2", this.xstartDate, this.xendDate);

        this.events = [];
        this.eventService.listEvents(this.xstartDate, this.xendDate, this.roomId, this.hostId).subscribe((events: Event[]) => {

            for (let event of events) {
                this.events.push(<Event>event);
            }

            this.refreshCalendar();
        });
    }

    onContextMenuItemClick($event, content) {
        switch ($event.args.item.id) {
            case "createAppointment":
                this.showCreateDialog($event, content);
                break;

            case "editAppointment":
                this.showEditDialog($event, content);
                break;
        }
    }

    showCreateDialog($event, content) {
        if (this.authService.isLoggedIn()) {
            this.saveEventTitle = 'calendar.event.create';

            this.createErrorMessages = {};

            let date = this.scheduler.getSelection();
            if (!date) {
                // exit in case the user wants to create an event over an existing event
                return;
            }

            this.model = new Event();
            this.model.startDate = new Date(date.from.toString());
            this.model.endDate = new Date(date.to.toString());
            this.model.eventType = EventTypeEnum.massage;
            this.model.eventStatus = EventStatusEnum.waiting;
            this.model.roomId = this.roomId;
            this.model.hostId = 3; // @TODO WE SHOULD NOT NEED A HOST
            this.model.attendeeId = this.authService.getLoggedUser().id; // @TODO get user id from logged user

            this.modalRef = this.modalService.open(content);
            this.modalRef.result.then((result) => {
                this.closeResult = `Closed with: ${result}`;
            }, (reason) => {
                this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
            });
        } else {
            this.redirectToLogin();
        }
    }

    showEditDialog($event, content) {
        if (this.authService.isLoggedIn()) {
            this.saveEventTitle = 'calendar.event.edit';

            this.createErrorMessages = {};

            this.model = this.events.find(e => e.id == $event.args.appointment.id);

            this.modalRef = this.modalService.open(content);
            this.modalRef.result.then((result) => {
                this.closeResult = `Closed with: ${result}`;
            }, (reason) => {
                this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
            });
        } else {
            this.redirectToLogin();
        }
    }

    cancelEvent() {
        this.model.eventStatus = EventStatusEnum.cancelled;
        this.saveEvent();
    }


    saveEvent() {
        // clear any previous errors
        this.createErrorMessages = {};

        // try to save
        this.eventService.save(this.model).subscribe(
            () => {
                // on save event success
                this.renderCalendar();
                // @TODO display success message
                this.modalRef.close();
            },
            error => {
                // on save event errors
                // @TODO handle generic errors
                if (error.status == 401) {
                    this.createErrorMessages = {'generic': ['Event.UserIsNotAuthenticated']};
                } else {
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

                }
            });

    }


    redirectToLogin() {
        if (!(this.authService.isLoggedIn())) {
            alert("You need to login if you want to make an appointment!");
            this.router.navigate(['/login']);
        }
    }
}
