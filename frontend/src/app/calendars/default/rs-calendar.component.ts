import {Router} from '@angular/router';
import {Component, ViewChild, AfterViewInit} from '@angular/core';
import {jqxSchedulerComponent} from '../../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import {NgbModal, ModalDismissReasons, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';
import {TranslateService, LangChangeEvent} from "@ngx-translate/core";
import { Subscription } from 'rxjs/Subscription';

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

    events: Event[] = [];
    model: Event = <Event> {};
    createErrorMessages: any = {};

    public date: Date = new $.jqx.date();

    public xstartDate: Date;
    public xendDate: Date;

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

    localization: any = {};

    private modalRef: NgbModalRef;
    private previousValues: any;
    subscription: Subscription;

    constructor(private router: Router, private translate: TranslateService, private eventService: EventService, private modalService: NgbModal, private authService: AuthService) {
    }

    ngAfterViewInit(): void {
        const t = this.translate;
        this.subscription = t.onLangChange.subscribe((event: LangChangeEvent) => {
            this.localization = {
                // separator of parts of a date (e.g. '/' in 11/05/1955)
                '/': '/',

                // separator of parts of a time (e.g. ':' in 05:44 PM)
                ':': ':',

                // the first day of the week (0 = Sunday, 1 = Monday, etc)
                firstDay: 0,
                days: {
                    // full day names
                    names: [
                        t.instant("calendar.days.names.Sunday"),
                        t.instant("calendar.days.names.Monday"),
                        t.instant("calendar.days.names.Tuesday"),
                        t.instant("calendar.days.names.Wednesday"),
                        t.instant("calendar.days.names.Thursday"),
                        t.instant("calendar.days.names.Friday"),
                        t.instant("calendar.days.names.Saturday")
                    ],

                    // abbreviated day names
                    namesAbbr: [
                        t.instant("calendar.days.namesAbbr.Sun"),
                        t.instant("calendar.days.namesAbbr.Mon"),
                        t.instant("calendar.days.namesAbbr.Tue"),
                        t.instant("calendar.days.namesAbbr.Wed"),
                        t.instant("calendar.days.namesAbbr.Thu"),
                        t.instant("calendar.days.namesAbbr.Fri"),
                        t.instant("calendar.days.namesAbbr.Sat")
                    ],

                    // shortest day names
                    namesShort: [
                        t.instant("calendar.days.namesShort.Su"),
                        t.instant("calendar.days.namesShort.Mo"),
                        t.instant("calendar.days.namesShort.Tu"),
                        t.instant("calendar.days.namesShort.We"),
                        t.instant("calendar.days.namesShort.Th"),
                        t.instant("calendar.days.namesShort.Fr"),
                        t.instant("calendar.days.namesShort.Sa")
                    ]
                },

                months: {
                    // full month names (13 months for lunar calendards -- 13th month should be "" if not lunar)
                    names: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", ""],
                    // abbreviated month names
                    namesAbbr: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", ""]
                },

                contextMenuEditAppointmentString: t.instant("Edit Appointment"),
                contextMenuCreateAppointmentString: t.instant("Create New Appointment"),
            };

            this.scheduler.localization(this.localization);
        });
    }

    ngOnDestroy() {
        // unsubscribe to ensure no memory leaks
        this.subscription.unsubscribe();
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
        this.date = new $.jqx.date(this.scheduler.date(), this.scheduler.timeZone());

        this.scheduler.view(this.view);
    }

    goToToday() {
        this.date = new $.jqx.date();
    }

    goBack() {
        this.date = this.addDaysInDirection(this.scheduler.date(), -1);
    }

    goForward() {
        this.date = this.addDaysInDirection(this.scheduler.date(), +1);
    }

    private addDaysInDirection(date, direction: number) {
        var i = this.views.find(e => e.type == this.view);
        var c = new $.jqx.date(date, this.scheduler.timeZone());

        let j = function () {
            while ((c.dayOfWeek() == 0 || c.dayOfWeek() == 6) && false === i.showWeekends) {
                c = c.addDays(direction * 1);
            }
            return c;
        };
        switch (this.view) {
            case"dayView":
            case"timelineDayView":
                c = c.addDays(direction * 1);
                c = j();
                break;

            case"weekView":
            case"timelineWeekView":
                c = c.addDays(direction * 7);
                break;

            case"agendaView":
                if (i.days) {
                    c = c.addDays(i.days);
                } else {
                    c = c.addDays(direction * 7);
                }
                break;
        }

        return c;
    }

    showCalendarsDate($event) {
        if (!this.previousValues || ($event.args.from.toString() !== this.previousValues.from.toString())) {
            this.previousValues = $event.args;

            let x: Date;

            x = $event.args.from.toDate();
            this.xstartDate = new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate() + (this.isView('weekView') ? 1 : 0),
                0, 0, 0
            ));

            x = $event.args.to.toDate();
            this.xendDate = new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate() + (this.isView('weekView') ? -3 : 0),
                23, 59, 59
            ));

            this.renderCalendar();
        }
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

    private renderCalendar() {
        if (!this.xstartDate || !this.xendDate) {
            return;
        }

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
