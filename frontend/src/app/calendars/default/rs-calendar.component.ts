import { forEach } from '@angular/router/src/utils/collection';
import {EventEmitter} from 'events';
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
    createErrorMessages = [];

    public startDate: Date;
    public selectedStartDate: Date;
    public selectedEndDate: Date;
    public roomId: number;
    public hostId: number;
    public eventId: number;
    public view = 'weekView';
    closeResult: string;
    
    generateAppointments(): any {
        let appointments = new Array();
        let appointment1 = {
            id: "id1",
            description: "George brings projector for presentations.",
            location: "",
            subject: "Quarterly Project Review Meeting",
            calendar: "Room 1",
            start: new Date(2017, 10, 23, 9, 0, 0),
            end: new Date(2017, 10, 23, 16, 0, 0)
        };
        appointments.push(appointment1);

        return appointments;
    };

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
        localData: this.generateAppointments()
    };

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

    dataAdapter: any = new jqx.dataAdapter(this.source);

    resources: any =
    {
        colorScheme: "scheme05",
        dataField: "calendar",
        source: new jqx.dataAdapter(this.source)
    };

    views: any[] = [
        {type: 'dayView', showWeekends: false, timeRuler: {scaleStartHour: 9, scaleEndHour: 18}},
        {type: 'weekView', showWeekends: false, timeRuler: {scaleStartHour: 9, scaleEndHour: 18}},
    ];

    constructor(private eventService: EventService, private modalService: NgbModal) {    
    }

    refreshCalendar() {
        let events = [];
        for (let event of this.events) {
            this.events.push(<any>{
                id: event.id,
                description: "George brings projector for presentations.",
                location: "",
                subject: "Quarterly Project Review Meeting",
                calendar: "Room " + event.roomId,
                start: new Date(event.startDate),
                end: new Date(event.endDate)
            });
        }        
        this.source.localData = events;
        this.dataAdapter = new jqx.dataAdapter(this.source);
    }

    showEditDialog(content) {
        
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

    private getDismissReason(reason: any): string {
        if (reason === ModalDismissReasons.ESC) {
            return 'by pressing ESC';
        } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
            return 'by clicking on a backdrop';
        } else {
            return  `with: ${reason}`;
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

    calendarUpdate(selectedRoom: Room) {
        this.roomId = selectedRoom.id;
        this.renderCalendar();
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

    createEvent() {
        this.eventService.createEvent(this.model.startDate = this.selectedStartDate, this.model.endDate = this.selectedEndDate,this.model.eventType = 0, this.model.roomId = 1, this.model.hostId = 3, this.model.attendeeId = 1, this.model.eventStatus = 4, this.model.notes).subscribe(
            () => {
                this.renderCalendar();
            },
            error => {
                if (error.error.errors.length > 0) {
                    this.createErrorMessages['generic'] = error.error.errors[0].errorCode;
                } else {
                    this.createErrorMessages['generic'] = error.error.message;
                }
               
                // for(let i=0; i<this.createErrorMessages.length; i++){
                //     this.createErrorMessages[i]= error.error.message;
                // }
                    
                
                // this.createErrorMessages = error.error.message;
            })
    }

    editEvent() {
        console.log(this.model);
        this.eventService.editEvent(this.model.startDate = this.selectedStartDate, this.model.endDate = this.selectedEndDate, this.model.id = 1, this.model.eventType = 0, this.model.roomId = 1, this.model.hostId = 3, this.model.attendeeId = 1, this.model.eventStatus = 4, this.model.notes).subscribe(
            () => {
                this.refreshCalendar();
            },
            error => {
              
                this.createErrorMessages = error.error.message;;
            })
    }

}

