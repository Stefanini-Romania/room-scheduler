import {Component, EventEmitter, Output, OnInit, AfterViewInit} from '@angular/core';
import {Availability} from '../../shared/models/availability.model';
import {NgbModal, NgbModalRef, NgbPaginationConfig, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import {HostAvailabilityForm} from '../../shared/hosts/host-availability-form/host-availability-form.component';
import {HostExceptionForm} from '../../shared/hosts/host-exception-form/host-exception-form.component';
import {User} from './../../shared/models/user.model';
import {HostService} from './../../shared/services/host.service';
import {HostAvailability} from './../../shared/hosts/host-availability/host-availability.component';

@Component({
    selector: 'admin-host-tab',
    templateUrl: './admin-host-tab.component.html',
    styleUrls: [],
    providers: [HostAvailability]  
})

export class AdminHostComponent {
    
    public today: Date;
    public model: Availability = <Availability>{};
    public selectedHost: User;

    constructor(private modalService: NgbModal, private hostService: HostService, public hostAvailability: HostAvailability){
        hostService.selectedHostChanged$.subscribe((host: User) => {
            this.selectedHost = host;
        });
    }

    ngAfterViewInit(){
        this.getToday();
        this.model.startDate = this.today;
    }

    getToday() {
        this.today = new Date();
        return this.today;
    }

    onAddAvailability() {
        const modalRef:NgbModalRef = this.modalService.open(HostAvailabilityForm, {});
        modalRef.componentInstance.host = this.selectedHost;
        modalRef.componentInstance.successfullAddAvailability.subscribe(() => {
            modalRef.close();
    })}

    onAddException() {
        const modalRef:NgbModalRef = this.modalService.open(HostExceptionForm, {});
        modalRef.componentInstance.host = this.selectedHost;
        modalRef.componentInstance.successfullAddException.subscribe(() => {
            modalRef.close();
        })
    }

    onHostChanged(selectedHost: User) {
        this.selectedHost = selectedHost; 
    }
}