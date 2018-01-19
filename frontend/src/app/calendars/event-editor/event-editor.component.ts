import {Component, Input, OnInit} from '@angular/core';
import {NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import {TranslateService} from "@ngx-translate/core";
import {ToastrService} from 'ngx-toastr';
import {EventStatusEnum} from '../../shared/models/event.model';
import {EventService} from '../shared/event.service';
import {Event} from '../../shared/models/event.model';
import {AuthService} from './../../auth/shared/auth.service';

@Component({
    selector: 'event-editor',
    templateUrl: './event-editor.component.html',
    providers: [EventService]
})

export class EventEditorComponent implements OnInit{
    @Input()
    public model: Event;
    public title: string;
    public errorMessages: any = {};

    constructor(public activeModal: NgbActiveModal, private toastr: ToastrService, private translate: TranslateService, private eventService: EventService, private authService: AuthService) {
    }

    ngOnInit() {
        this.title = this.model.id ? 'calendar.event.edit': 'calendar.event.create';
        
    }

    cancelEvent() {
        this.model.eventStatus = EventStatusEnum.cancelled;
        this.toastr.warning(
            this.translate.instant('calendar.event.canceled'), '',
            {positionClass: 'toast-bottom-right'}
        );
        return this.saveEvent();
    }
    

    saveEvent() {
        // clear any previous ecxrrors
        this.errorMessages = {};
       

        // try to save
        return this.eventService.save(this.model).subscribe(
            () => {
                if (this.model.eventStatus != EventStatusEnum.cancelled)
                {
                this.toastr.success(
                    this.translate.instant('calendar.event.saved'), '',
                    {positionClass: 'toast-bottom-right'}
                );}
                this.activeModal.close();

            },
            error => {
                // on save event errors
                if (error.status == 401) {
                    this.errorMessages = {'generic': ['Event.UserIsNotAuthenticated']};
                    this.authService.logout();
                } else {
                    this.errorMessages = {'generic': [error.error.message]};

                    // build error message
                    for (let e of error.error.errors) {
                        let field = 'generic';
                        if (['StartDate', 'EndDate'].indexOf(e.field) >= 0) {
                            field = e.field;
                        }
                        if (!this.errorMessages[field]) {
                            this.errorMessages[field] = [];
                        }
                        this.errorMessages[field].push(e.errorCode);
                    }
                }
            });
    }
}
