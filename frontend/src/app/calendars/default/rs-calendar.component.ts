import {User} from '../../shared/models/user.model';
import {Component, ViewChild, OnInit,AfterViewInit, OnDestroy} from '@angular/core';
import {jqxSchedulerComponent} from './temp-hack/angular_jqxscheduler';
import {NgbModal, NgbModalRef} from '@ng-bootstrap/ng-bootstrap';
import {TranslateService} from "@ngx-translate/core";
import {Subscription} from 'rxjs/Subscription';

import {LoginFormComponent} from '../../users/login-form/login-form.component';
import {EventService} from '../shared/event.service';
import {RoomSelector} from '../../rooms/room-selector/room-selector.component';
import {Room} from '../../shared/models/room.model';
import {Event} from '../../shared/models/event.model';
import {AuthService} from '../../auth/shared/auth.service';
import {EventTypeEnum} from '../../shared/models/event.model';
import {EventStatusEnum} from '../../shared/models/event.model';
import {DialogService} from '../../shared/services/dialog.service';
import {EventEditorComponent} from '../event-editor/event-editor.component';
import * as CalendarSettings from './calendar-settings.json';
import {RoleEnum} from './../../shared/models/role.model';

@Component({
    selector: 'rs-calendar-component',
    templateUrl: './rs-calendar.component.html',
    providers: [EventService, RoomSelector]
})

export class RSCalendarComponent implements OnInit, AfterViewInit, OnDestroy {
    @ViewChild('schedulerReference') scheduler: jqxSchedulerComponent;

    events: Event[] = [];
    users: User[] = [];
    availabilities: any;
    hosts: any[] = [];
    availabilityHostGroupName: string;

    public date: Date = new jqx.date();
    public startDate: Date;
    public endDate: Date;
    public selectedRoom: Room;
    public hostId: number;
    private previousValues: any;
    public RoleEnum: User;

    calendarSettings = <any>CalendarSettings;
    dataAdapter = new jqx.dataAdapter(this.calendarSettings["source"]);
    subscription: Subscription;

    constructor(private translate: TranslateService,
        private dialogService: DialogService, private eventService: EventService, private modalService: NgbModal,
        private authService: AuthService){
    }
              

    ngOnInit() {
        this.subscription =  this.translate.onLangChange.subscribe(() => {
            this.updateCalendarTranslations();
        });
    }

    ngAfterViewInit(): void {
        this.updateCalendarTranslations();
        this.goToToday(); 
    }

    ngOnDestroy() {
        // unsubscribe to ensure no memory leaks
        this.subscription.unsubscribe();
    }

    refreshCalendar() {
        let events = [];
        let eventType: string;
        for (let event of this.events) {
            let readOnly = false;
            switch(event.eventType){
                case EventTypeEnum.availability:
                    if (event.eventStatus == 1) {
                        eventType = this.translate.instant("calendar.eventType.availabilty");
                        readOnly = true;
                    }
                    else {
                        eventType = event.hostName;
                        readOnly = false;
                    }
                    
                    break;
                case EventTypeEnum.massage: 
                    if (this.authService.isLoggedIn()) {
                        if (this.authService.getLoggedUser().hasRole(RoleEnum.admin) || this.authService.getLoggedUser().hasRole(RoleEnum.host)) {                  
                            eventType = this.translate.instant("calendar.eventType.massage") + ": " + event.attendeeName;
                        }
                        else eventType = this.translate.instant("calendar.eventType.massage");  
                    }
                    else eventType = this.translate.instant("calendar.eventType.massage");
                    break;
            }

            if (this.authService.isLoggedIn()) {
                
                if (event.eventType == EventTypeEnum.massage) {

                    if ((event.attendeeId !== this.authService.getLoggedUser().id) && (this.authService.getLoggedUser().departmentId != null)) {
                        // @TODO allow admins and hosts to edit anyway
                        readOnly = true;
                    }   
                    if (this.authService.getLoggedUser().hasRole(RoleEnum.admin) || this.authService.getLoggedUser().hasRole(RoleEnum.host)){
                        readOnly = false;
                    }
                }

                if ((new Date(event.startDate) <= new Date())&&(this.authService.getLoggedUser().departmentId != null)){
                    if (this.authService.getLoggedUser().hasRole(RoleEnum.admin) || this.authService.getLoggedUser().hasRole(RoleEnum.host)){
                        readOnly = false;
                    }
                    else readOnly = true;
                }
            }

            events.push(<any>{
                id: event.id,
                description: event.notes,
                location: "",
                subject: eventType,
                calendar: "Room " + event.roomId,
                draggable: false,
                resizable: false,
                readOnly: readOnly,              
                start: new Date(event.startDate),
                end: new Date(event.endDate),
                allDay: false
            });
        }

        //console.log(events);

        this.calendarSettings["source"].localData = events;
        this.dataAdapter = new jqx.dataAdapter(this.calendarSettings["source"]);
    }

    updateCalendarTranslations() {
        const t = this.translate;

        this.calendarSettings["localization"] = {
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

            contextMenuEditAppointmentString: t.instant("calendar.event.edit"),
            contextMenuCreateAppointmentString: t.instant("calendar.event.create"),
        };

        this.refreshCalendar();
    }

    isView(view: string): boolean {
        return view == this.calendarSettings["view"];
    }

    setView(view: string) {
        this.calendarSettings["view"] = view;
        this.date = new jqx.date(this.scheduler.date(), this.scheduler.timeZone());

        this.scheduler.view(view);
    }

    goToToday() {
        this.date = new jqx.date();
    }

    goBack() {
        this.date = this.addDaysInDirection(this.scheduler.date(), -1);
    }

    goForward() {
        this.date = this.addDaysInDirection(this.scheduler.date(), +1);
    }

    private addDaysInDirection(date, direction: number) {
        let i = Array<any>(this.scheduler.views).find(e => e.type == this.calendarSettings["view"]);
        let c = new jqx.date(date, this.scheduler.timeZone());

        let j = function () {
            while ((c.dayOfWeek() == 0 || c.dayOfWeek() == 6) && false === i.showWeekends) {
                c = c.addDays(direction * 1);
            }
            return c;
        };

        switch (this.calendarSettings["view"]) {
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

    updateCalendarDates($event) {
        if (!this.previousValues || ($event.args.from.toString() !== this.previousValues.from.toString())) {
            this.previousValues = $event.args;

            let x: Date;

            x = $event.args.from.toDate();
            this.startDate = new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate() + (this.isView('weekView') ? 1 : 0),
                0, 0, 0
            ));

            x = $event.args.to.toDate();
            this.endDate = new Date(Date.UTC(x.getFullYear(), x.getMonth(), x.getDate() + (this.isView('weekView') ? -3 : 0),
                23, 59, 59
            ));

            this.renderCalendar();
        }
    }

    onRoomChanged(selectedRoom: Room) {
        this.selectedRoom = selectedRoom;
        this.renderCalendar();
    }

    private renderCalendar() {
        if (!this.startDate || !this.endDate || !this.selectedRoom) {
            return;
        }

        this.events = [];
        let hosts = {};
        this.eventService.listEvents(this.startDate, this.endDate, this.selectedRoom.id, this.hostId).subscribe((events: Event[]) => {

            for (let event of events) {
                if (event.eventType == EventTypeEnum.availability && event.eventStatus == 0) {
                    // compute host availabilities
                    if (!hosts[event.hostName]) {
                        hosts[event.hostName] = {};
                    }

                    const startDate = new Date(event.startDate);
                    const day = this.calendarSettings["localization"].days.namesAbbr[startDate.getDay()];
                    if (!hosts[event.hostName][day]) {
                        hosts[event.hostName][day] = [];
                    }
                    /*const endDate = new Date(event.endDate);

                    const startHour = startDate.getHours() + ":" + startDate.getMinutes();
                    const endHour = endDate.getHours() + ":" + endDate.getMinutes();

                    hosts[event.host][day].push(startHour + " - " + endHour);*/
                    hosts[event.hostName][day].push(event);

                } else if(event.eventType == EventTypeEnum.massage || (event.eventType == EventTypeEnum.availability && event.eventStatus == 1))
                    this.events.push(<Event>event); {
                }
            }

            this.availabilities = hosts;
            //console.log(this.availabilities);

            this.refreshCalendar();
            
        });
    }

    ContextMenuOpen(event: any): void {
        if (!event.args.appointment) {
             event.args.menu.jqxMenu('showItem', 'createAppointment');
             //event.args.menu.jqxMenu('showItem', 'editAppointment');
        }
        else {
             event.args.menu.jqxMenu('hideItem', 'createAppointment');
             //event.args.menu.jqxMenu('hideItem', 'editAppointment');
        }
    }

    onContextMenuItemClick($event) {
        switch ($event.args.item.id) {
            case "createAppointment":
                this.showCreateDialog();
                break;

            case "editAppointment":
                this.showEditDialog($event);
                break;
        }
    }

    test($event) {
        /*let modalRef = this.dialogService.alert("This is a test");
        modalRef.result.then(value => {
            console.log("V=", value);
        }, reason => {
            console.log("D=", reason);
        });
        if (1)return;*/

        let model = new Event();
        model.startDate = new Date();
        model.endDate = new Date();
        model.eventType = EventTypeEnum.massage;
        model.eventStatus = EventStatusEnum.waiting;
        model.roomId = 1;
        model.hostId = 3; // @TODO WE SHOULD NOT NEED A HOST
        model.attendeeId = 1; // this will be removed after backend will put the attendeeId from server (Current User)

        this.openEventEditor(model);
    }

    private openEventEditor(model: Event) {
        const modalRef:NgbModalRef = this.modalService.open(EventEditorComponent);
        let initialDate = new Date(model.dateCreated);
        
        initialDate.setHours(initialDate.getHours()+2);
        model.dateCreated = initialDate;
        modalRef.componentInstance.model = model;
    
        modalRef.result.then(() => {
            this.renderCalendar();
        });
    }

    showCreateDialog() {
        if (!this.authService.isLoggedIn()) {
            return this.redirectToLogin();
        }

        if (this.authService.getLoggedUser().hasPenaltiesForRoom(this.selectedRoom)) {
            return this.dialogService.alert({message: "User.YouArePenalised", title: "User.Title"});
        }

        let date = this.scheduler.getSelection();
        if (!date) {
            // exit in case the user wants to create an event over an existing event
            return;
        }

        let model = new Event();
        model.startDate = new Date(date.from.toString());
        model.endDate = new Date(date.to.toString());
        model.eventType = EventTypeEnum.massage;
        model.eventStatus = EventStatusEnum.waiting;
        model.roomId = this.selectedRoom.id;
        model.attendeeId = this.authService.getLoggedUser().id;

        this.openEventEditor(model);
    }

    showEditDialog($event) {
        if (!this.authService.isLoggedIn()) {
            this.redirectToLogin();
        }

        let model = this.events.find(e => e.id == $event.args.appointment.id);
        // model.dateCreated = new Date(model.dateCreated.getHours()+2);
        this.openEventEditor(model);
    }

    redirectToLogin() {
        if (!(this.authService.isLoggedIn())) {
            const modalRef:NgbModalRef = this.modalService.open(LoginFormComponent);
            modalRef.componentInstance.successfullLogin.subscribe(() => {
                modalRef.close();
            });
            modalRef.result.then(() => {
                this.renderCalendar();
            })
            .catch(() => {});
        }
    }

    renderAppointment = (data) => {
        if (!data.appointment || !this.events.length) {
            return;
        }

        let event = this.events.find(e => e.id == data.appointment.id);
        if (event.eventType == EventTypeEnum.availability) {
            if (event.eventStatus == 0) {
                data.style = '#E0E0E0'; //grey
            }
        }
        else {
            data.style = '#004e9e'; //blue
        }

        if ((this.authService.isLoggedIn())) {
            if (event.attendeeId == this.authService.getLoggedUser().id) {
                data.style = "#d7dd3b"; //green?
            }
            if (!this.authService.getLoggedUser().hasRole(RoleEnum.attendee) && event.eventType == EventTypeEnum.massage && event.eventStatus == EventStatusEnum.absent){
                data.style = '#C00000'; //red
            }
        }
        
        return data;
    }


}
