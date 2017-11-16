import {Component, ViewChild} from '@angular/core';
import {jqxSchedulerComponent} from '../../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import {EventService} from '../shared/event.service';
import {RoomSelector} from '../../rooms/room-selector/room-selector.component';
import {Room} from '../../shared/models/room.model';
import { Event } from '../../shared/models/event.model';
import {AuthService} from '../../auth/shared/auth.service';
import { EventTypeEnum } from '../../shared/models/event.model';
import { EventStatusEnum } from '../../shared/models/event.model';

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

    constructor(private eventService: EventService, private modalService: NgbModal, private authService: AuthService) {
    }

    ngAfterViewInit(): void {
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
            names: ['Duminică' , 'Luni', 'Marți', 'Miercuri', 'Joi', 'Vineri', 'Sâmbătă'],
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

    showCalendarsDate(){  
        let a = new Date();
        
        if (a.getDay() == 1)
        {
            this.calendarsDateFrom = new Date(this.scheduler.date().toString());
            this.calendarsDateTo = new Date(this.scheduler.date().addDays(4).toString());
        }
        if (a.getDay() == 2)
        {
            this.calendarsDateFrom = new Date(this.scheduler.date().addDays(-1).toString());
            this.calendarsDateTo = new Date(this.scheduler.date().addDays(3).toString());
        }
        if (a.getDay() == 3)
        {
            this.calendarsDateFrom = new Date(this.scheduler.date().addDays(-2).toString());
            this.calendarsDateTo = new Date(this.scheduler.date().addDays(2).toString());
        }
        if (a.getDay() == 4)
        {
            this.calendarsDateFrom = new Date(this.scheduler.date().addDays(-3).toString());
            this.calendarsDateTo = new Date(this.scheduler.date().addDays(1).toString());
        }
        if (a.getDay() == 5)
        {
            this.calendarsDateFrom = new Date(this.scheduler.date().addDays(-4).toString());
            this.calendarsDateTo = new Date(this.scheduler.date().toString());
        }
        if (a.getDay() == 6)
        {
            this.calendarsDateFrom = new Date(this.scheduler.date().addDays(-5).toString());
        }
        if (a.getDay() == 7)
        {
            this.calendarsDateFrom = new Date(this.scheduler.date().addDays(-6).toString());
        }     
                     
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

    getMonday(){
        let start = new Date();
        let day = start.getDay();
        // let diff = start.getDay() - ( day == 1 ? 6:1); // adjust when day is monday
       
        while(day!=1){
            day = day-1;
        }
        start.setDate(day);
        console.log(start);
       
        // return start.getDate(diff).toString();
       

    }

    getFirstDay(){
      
        
    }

    private renderCalendar() {
        this.events = [];
        const days = this.isView('weekView') ? 7 : 1;

        let endDate = new Date();
        endDate.setTime(this.startDate.getTime() + days * 86400000).toString();

        this.eventService.listEvents(this.startDate, endDate, this.roomId, this.hostId).subscribe((events: Event[]) => {

            for (let event of events) {
                this.events.push(<Event>event);
            }

            this.refreshCalendar();
        });
    }

    showCreateDialog(content){
        if(this.authService.isLoggedIn()){

            let date = this.scheduler.getSelection();
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

            this.modalService.open(content).result.then((result) => {
                this.closeResult = `Closed with: ${result}`;
            }, (reason) => {
                this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
            });
            this.createErrorMessages = {};
        } else {
            //@TODO redirect login
        }
    }

    showEditDialog(content, $event) {
        if(this.authService.isLoggedIn()){
                this.model = this.events.find(e => e.id == $event.args.appointment.id)
                this.selectedStartDate = this.model.startDate;
                this.selectedEndDate = this.model.endDate;

                this.saveEventTitle = 'calendar.event.edit';

                this.modalService.open(content).result.then((result) => {
                    this.closeResult = `Closed with: ${result}`;
                }, (reason) => {
                    this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
                });
                this.createErrorMessages = {};           
         
        } else {
            this.authService.notLoggedIn();
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
                this.renderCalendar();
                // @TODO display success message
            },
            error => {
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

    sendToLogin() {
        this.authService.notLoggedIn();
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
