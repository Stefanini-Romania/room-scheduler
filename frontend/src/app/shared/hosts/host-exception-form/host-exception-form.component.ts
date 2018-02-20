import {Component, NgModule, Input, Output, EventEmitter} from '@angular/core'
import {TranslateService} from "@ngx-translate/core";
import {NgbActiveModal, NgbModalRef, NgbModal} from '@ng-bootstrap/ng-bootstrap';
import {FormBuilder, FormGroup, FormControl} from '@angular/forms';
import {SelectControlValueAccessor} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';
import {NgbDatepickerConfig, NgbDateStruct, NgbTimeStruct} from '@ng-bootstrap/ng-bootstrap';

import {User} from '../../models/user.model';
import {Availability} from '../../models/availability.model';
import {HostService} from './../../services/host.service';

@Component({
    selector: 'host-exception-form',
    templateUrl: './host-exception-form.component.html',
    providers: [NgbDatepickerConfig]
})

export class HostExceptionForm {

    @Input() 
    host: User;

    @Output()
    successfullAddException = new EventEmitter;
    successfullEditException = new EventEmitter;

    public model: Availability = <Availability>{};
    public displayDate = new Date();
    public minuteStep = 30;
    public addExcept: boolean;
    public errorMessages: any = {};
    public exceptionStartDate; 
    public exceptionEndDate;
    public title: string;
    public startHour: NgbTimeStruct;
    public endHour: NgbTimeStruct;
    public seconds = false;

    constructor(public hostService: HostService, private formBuilder: FormBuilder, private translate: TranslateService, public activeModal: NgbActiveModal, private toastr: ToastrService, private datePickerConfig: NgbDatepickerConfig){
        datePickerConfig.markDisabled = (date: NgbDateStruct) => {
            const day = new Date(date.year, date.month - 1, date.day);
            return day.getDay() === 0 || day.getDay() === 6;
          };
    }

    //validation for hours between 9 and 18 availability
    ctrl = new FormControl('', (control: FormControl) => {
        const value = control.value;
    
        if (!value) {
            return null;
        }
    
        if (value.hour < 9 || value.hour > 17) {
            return {tooEarly: true};
        }
    
        return null;
    });

    ngOnInit() {
        this.title = this.model.id ? 'Exception.edit': 'Exception.add';
        // if (this.model.id) {  
        //     this.model.startDate = new Date();    
        //     JSON.stringify(this.model.startDate);
        //     this.displayDate = new Date(this.model.startDate);
        // }
        // else {
        //     this.displayDate = new Date();
        // }

        if (this.model.id) {
            let startH = (new Date(this.model.startDate)).getHours();
            let startM = (new Date(this.model.startDate)).getMinutes();
            let endH = (new Date(this.model.endDate)).getHours();
            let endM = (new Date(this.model.endDate)).getMinutes();
            this.startHour = {hour: startH, minute: startM, second: 0};
            this.endHour = {hour: endH, minute: endM, second: 0};


        } 
    }

    dateFormat(Availability) {
        if (this.model.id) {   
            this.exceptionStartDate = new Date(this.model.startDate);
            this.exceptionStartDate.setHours(this.startHour.hour);
            JSON.stringify(this.exceptionStartDate);    

            this.model.endDate = new Date(this.exceptionStartDate.getFullYear(), this.exceptionStartDate.getMonth(),this.exceptionStartDate.getDate(), this.endHour["hour"], this.endHour["minute"]) ;
            this.exceptionEndDate = this.model.endDate;
            JSON.stringify(this.exceptionEndDate);
        }
        else {
            this.exceptionStartDate = new Date(this.model.startDate["year"], this.model.startDate["month"]-1, this.model.startDate["day"], this.startHour["hour"], this.startHour["minute"], 0);
            JSON.stringify(this.exceptionStartDate);
        
            this.model.endDate = new Date(this.exceptionStartDate.getFullYear(), this.exceptionStartDate.getMonth(),this.exceptionStartDate.getDate(),  this.endHour["hour"], this.endHour["minute"]) ;
            this.exceptionEndDate = this.model.endDate;
        } 
    }      

    addException(){
        if(this.model.startDate && this.startHour && this.endHour) {
            this.dateFormat(Availability);
            this.hostService.AddHostException(
                this.exceptionStartDate,
                this.exceptionEndDate,
                this.host.id).subscribe
                    (() => {},
                    error => {
                        if(error.status==200){
                            this.successfullAddException.emit();
                            this.toastr.success(
                                this.translate.instant('Exception.successfully.added'), '',
                                {positionClass: 'toast-bottom-right'}
                            );
                        }
                        else {
                            this.errorMessages = {'generic': [error.error.message]};
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
        else this.addExcept=false;
    }

    editException() {   
        this.dateFormat(Availability);  
        this.hostService.EditHostException(
            this.model.id, 
            this.exceptionStartDate,
            this.exceptionEndDate,
            this.model.status).subscribe(
                () => {},
                error => {
                    if (error.status == 200){
                        this.successfullEditException.emit();
                        this.toastr.success(
                            this.translate.instant('Exception.successfully.edit'), '',
                            {positionClass: 'toast-bottom-right'}
                        );
                    }
                    else {
                        this.errorMessages = {'generic': [error.error.message]};
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

    removeException() {
        this.model.status = 1;
        this.toastr.warning(
            this.translate.instant('Exception.removed'), '',
            {positionClass: 'toast-bottom-right'}
        );
        this.activeModal.close();
        return this.editException();   
    }
}
