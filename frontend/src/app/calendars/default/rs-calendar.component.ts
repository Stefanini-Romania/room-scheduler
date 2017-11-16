import {Router} from '@angular/router';
import {Component, ViewChild} from '@angular/core';
import {jqxSchedulerComponent} from '../../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import {NgbModal, ModalDismissReasons, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';
import {EventService} from '../shared/event.service';
import {RoomSelector} from '../../rooms/room-selector/room-selector.component';
import {Room} from '../../shared/models/room.model';
import {Event} from '../../shared/models/event.model';
import {AuthService} from '../../auth/shared/auth.service';
import {Pipe, PipeTransform, Output} from '@angular/core';
import {EventTypeEnum} from '../../shared/models/event.model';
import {EventStatusEnum} from '../../shared/models/event.model';
import {ResourceLoader} from '@angular/compiler';

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

    public startDate: Date;
    public endDate: Date;
    public selectedStartDate: Date;
    public selectedEndDate: Date;
    public roomId: number;
    public hostId: number;
    public eventId: number;
    public saveEventTitle: string;
    public calendarsDateFrom: Date;
    public calendarsDateTo: Date;
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

    constructor(private router: Router, private eventService: EventService, private modalService: NgbModal, private authService: AuthService) {
    }

    ngAfterViewInit(): void {
        this.scheduler.ensureAppointmentVisible('id1');

        this.startDate = new Date();
        this.renderCalendar();

    }

    localization = {
        // separator of parts of a date (e.g. '/' in 11/05/1955)
        '/': '/',
        // separator of parts of a time (e.g. ':' in 05:44 PM)
        ':': ':',
        // the first day of the week (0 = Sunday, 1 = Monday, etc)
        firstDay: 0,
        days: {
            // full day names
            names: ['Duminică', 'Luni', 'Marți', 'Miercuri', 'Joi', 'Vineri', 'Sâmbătă'],
            // abbreviated day names
            //namesAbbr: ['Sonn', 'Mon', 'Dien', 'Mitt', 'Donn', 'Fre', 'Sams'],
            // shortest day names
            namesShort: ['Du', 'Lu', 'Ma', 'Mi', 'Jo', 'Vi', 'Sâ']
        },

        // months: {
        //     // full month names (13 months for lunar calendards -- 13th month should be '' if not lunar)
        //     names: ['Ianuarie', 'Februarie', 'Martie', 'Aprilie', 'Mai', 'iunie', 'Iulie', 'August', 'Septembrie', 'Octombrie', 'Noiembrie', 'Decembrie', ''],
        //     // abbreviated month names
        //     namesAbbr: ['Ian', 'Feb', 'Mar', 'Apr', 'Mai', 'Iun', 'Iul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec', '']
        // }
    };

    private modalRef: NgbModalRef;

    public refresh() {
        this.renderCalendar();
    }

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
        this.startDate = new Date();
        this.renderCalendar();
    }

    showCalendarsDate($event) {
        let x: Date;

        x = $event.args.from.toDate();
        this.xstartDate = new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate() + 1, 0, 0, 0));

        x = $event.args.to.toDate();
        this.xendDate = new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate() - 3, 23, 59, 59));
        console.log("HERE1", this.xstartDate, this.xendDate);
    }


    goBack() {
        const days = this.isView('weekView') ? 7 : 1;
        this.startDate = new Date(this.scheduler.date().addDays(-days).toString());
        this.renderCalendar();
    }

    goForward() {
        const days = this.isView('weekView') ? 7 : 1;
        this.startDate = new Date(this.scheduler.date().addDays(days).toString());
        this.renderCalendar();

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
            this.createErrorMessages = {};

            let date = this.scheduler.getSelection();
            if (!date) {
                // exit in case the user wants to create an event over an existing event
                return;
            }

            this.selectedStartDate = new Date(date.from.toString());
            this.selectedEndDate = new Date(date.to.toString());

            this.saveEventTitle = 'calendar.event.create';
            this.model = new Event();
            this.model.startDate = this.selectedStartDate;
            this.model.endDate = this.selectedEndDate;
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
            this.model = this.events.find(e => e.id == $event.args.appointment.id)
            this.selectedStartDate = this.model.startDate;
            this.selectedEndDate = this.model.endDate;

            this.saveEventTitle = 'calendar.event.edit';

            this.modalRef = this.modalService.open(content);
            this.modalRef.result.then((result) => {
                this.closeResult = `Closed with: ${result}`;
            }, (reason) => {
                this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
            });
            this.createErrorMessages = {};

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


// export class UtcDatePipe implements PipeTransform {

//       transform(value: string): any {

//         if (!value) {
//           return '';
//         }

//         const dateValue = new Date(value);

//         const dateWithNoTimezone = new Date(
//           dateValue.getUTCFullYear(),
//           dateValue.getUTCMonth(),
//           dateValue.getUTCDate(),
//           dateValue.getUTCHours(),
//           dateValue.getUTCMinutes(),
//           dateValue.getUTCSeconds()
//         );

//         return dateWithNoTimezone;
//       }
// }
