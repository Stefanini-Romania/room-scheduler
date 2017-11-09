import {EventEmitter} from 'events';
import {Component, ViewChild} from '@angular/core';
import {jqxSchedulerComponent} from '../../../../node_modules/jqwidgets-framework/jqwidgets-ts/angular_jqxscheduler';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
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
    public createErrorMessage: string = '';
    public startDate: Date;
    public selectedStartDate: Date;
    public selectedEndDate: Date;
    public roomId: number;
    public hostId: number;
    public eventId: number;

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

    closeResult: string;
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


    constructor(private eventService: EventService, private modalService: NgbModal) {
    }

    showEditDialog(content) {
        
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
        console.log(this.model);
        this.eventService.createEvent(this.model.startDate = this.selectedStartDate, this.model.endDate = this.selectedEndDate, this.model.eventType = 0, this.model.roomId = 1, this.model.hostId = 3, this.model.attendeeId = 1, this.model.eventStatus = 4, this.model.notes).subscribe(
            () => {
                this.renderCalendar();
            },
            error => {
                this.createErrorMessage = error.error.message;
            })
    }

    editEvent() {
        console.log(this.model);
        this.eventService.editEvent(this.model.startDate = this.selectedStartDate, this.model.endDate = this.selectedEndDate, this.model.id = 1, this.model.eventType = 0, this.model.roomId = 1, this.model.hostId = 3, this.model.attendeeId = 1, this.model.eventStatus = 4, this.model.notes).subscribe(
            () => {
                this.refreshCalendar();
            },
            error => {
                this.createErrorMessage = "Unable to create event";
            })
    }



    refreshCalendar() {
        this.source.localData = this.events;
        this.dataAdapter = new jqx.dataAdapter(this.source);
    }

}

