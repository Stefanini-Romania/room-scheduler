import {Component, EventEmitter, Output, OnInit, AfterViewInit} from '@angular/core';
import {Availability} from '../../shared/models/availability.model';

@Component({
    selector: 'admin-host-tab',
    templateUrl: './admin-host-tab.component.html',
    styleUrls: [],
    
})

export class AdminHostComponent {
    
    public today: Date;
    public model: Availability = <Availability>{};

    constructor(){

    }

    ngAfterViewInit(){
        this.getStartOfWeek();
        this.getToday();
        this.model.startDate = this.today;

    }

    getToday() {
        this.today = new Date();
        return this.today;
    }
    
    getStartOfWeek(){
        this.model.startDate = new Date();
        let dayOfWeek = this.model.startDate.getDay();
        let currentDate = this.model.startDate.getDate();
        while(dayOfWeek!==1){
            dayOfWeek--;
            currentDate=currentDate-1;
        }
        this.model.startDate.setDate(currentDate);
        this.model.endDate = new Date(this.model.startDate.getFullYear(), this.model.startDate.getMonth(), this.model.startDate.getDate()+4);

    }
}