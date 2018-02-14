import {Component, EventEmitter, Output, OnInit, AfterViewInit} from '@angular/core';
import {TranslateService} from "@ngx-translate/core";
import {ToastrService} from 'ngx-toastr';

import {SystemParametersService} from '../shared/system-parameters.service';
import {Settings} from '../../shared/models/settings.model';

@Component({
    selector: 'admin-system-parameters',
    templateUrl: './admin-system-parameters.component.html',
    styleUrls: [],
    providers: [SystemParametersService]
    
})

export class AdminSystemParameters implements AfterViewInit{

    public settingslist: Settings[];
    public model: Settings = <Settings> {};
    public modelForm: Settings = <Settings>{};
    public errorMessages: any = {};
    public editable: boolean;
    

    constructor(private systemParametersService: SystemParametersService, private translate: TranslateService, private toastr: ToastrService){
        
    }

    ngOnInit() {
        this.modelForm.id = this.model.id;
        this.modelForm.varName = this.model.varName;
        this.modelForm.value = this.model.value;

    }

    ngAfterViewInit(): void {
        this.listSettings();
        
    }

    listSettings(){
        this.settingslist = [];
        this.systemParametersService.listParameters().subscribe((settingslist: any) =>{
            for (let setting of settingslist){
                this.settingslist.push(<Settings>setting);
                this.modelForm.id = setting.id;
                this.modelForm.varName = setting.varName;
                this.modelForm.value = setting.value;
            }
        });
       
    }

    editSettings() {

        this.systemParametersService.editParameters(this.modelForm.id, this.modelForm.varName, this.modelForm.value).subscribe(
            () => {           
            },
            error => {
                this.model = this.modelForm;
                if(error.status==200){
                    this.listSettings();
                    this.toastr.success(
                        this.translate.instant('SystemParameters.Save'), '',
                        {positionClass: 'toast-bottom-right'}        
                )}

                else if(error.status!=200){
                    // this.errorMessages = error.error.message;
                    this.errorMessages = {'generic': [error.error.message]};
                    
                                // build error message
                                for (let e of error.error.errors) {
                                    let field = 'generic';         
                                    if (['Value'].indexOf(e.field) >= 0) {
                                        field = e.field;
                                    }
                                    if (!this.errorMessages[field]) {
                                        this.errorMessages[field] = [];
                                    }
                                    this.errorMessages[field].push(e.errorCode);
                                }
                                this.toastr.warning(
                                    this.translate.instant('SystemParameters.Error'), '',
                                    {positionClass: 'toast-bottom-right'}        
                            );
                }
                
    });    
        this.editable=false;
    }

    makeEditable(model: Settings) {
        this.modelForm=model;
        this.editable = true; 
      }
      
}