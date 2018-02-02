import {Component, EventEmitter, Output, OnInit, AfterViewInit} from '@angular/core';
import {Availability} from '../../shared/models/availability.model';
import {NgbModal, NgbModalRef, NgbPaginationConfig, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import {HostAvailabilityForm} from '../../shared/hosts/host-availability-form/host-availability-form.component';
import {User} from './../../shared/models/user.model';
import {HostService} from './../../hosts/shared/host-selector.service';
@Component({
    selector: 'admin-host-tab',
    templateUrl: './admin-host-tab.component.html',
    styleUrls: [],
    providers: [HostService]
    
})

export class AdminHostComponent {
    
    public today: Date;
    public model: Availability = <Availability>{};
    public selectedHost: User;

    constructor(private modalService: NgbModal, public hostService: HostService){
        hostService.selectedHostChanged$.subscribe((host: User) => {
            this.selectedHost;
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
        const modalRef:NgbModalRef = this.modalService.open(HostAvailabilityForm);
    }

    onHostChanged(selectedHost: User) {
        this.selectedHost = selectedHost;
        //this.listAvailabilities();
    }
}