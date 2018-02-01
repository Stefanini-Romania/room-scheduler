import {Component, EventEmitter, Output, OnInit, AfterViewInit} from '@angular/core';
import {Availability} from '../../shared/models/availability.model';
import {NgbModal, NgbModalRef, NgbPaginationConfig, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';

import {HostAvailabilityForm} from '../../shared/hosts/host-availability-form/host-availability-form.component';
@Component({
    selector: 'admin-host-tab',
    templateUrl: './admin-host-tab.component.html',
    styleUrls: [],
    
})

export class AdminHostComponent {
    
    public today: Date;
    public model: Availability = <Availability>{};

    constructor(private modalService: NgbModal){

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
}