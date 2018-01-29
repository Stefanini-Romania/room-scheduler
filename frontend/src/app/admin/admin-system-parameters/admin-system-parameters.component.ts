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
    public errorMessages: any = {};
    public editable: boolean;
    

    constructor(private systemParametersService: SystemParametersService, private translate: TranslateService, private toastr: ToastrService){
        
    }
    ngOnInit(){
    
    }

    ngAfterViewInit(): void {
        this.listSettings();
    }

    listSettings(){
        this.settingslist = [];
        this.systemParametersService.listParameters().subscribe((settingslist: any) =>{
            for (let setting of settingslist){
                this.settingslist.push(<Settings>setting);
                this.model.id = setting.id;
                this.model.varName = setting.varName;
                this.model.value = setting.value;
            }
        });
       
    }

    editSettings() {
        this.systemParametersService.editParameters(this.model.id, this.model.varName, this.model.value).subscribe(
            () => {           
            },
            error => {
                if(error.status==200){
                    this.listSettings();
                    this.toastr.success(
                        this.translate.instant('SystemParameters.Save'), '',
                        {positionClass: 'toast-bottom-right'}        
                )}

                else {
                    this.errorMessages = error.error.message;
                }
                
    }); 
            
        this.editable=false;
    }

    makeEditable(model: Settings) {
        this.settingslist = this.settingslist.map((setting) => {
            if (setting.id) { 
                this.editable = true; 
            }
            else { 
                this.editable = false; 
            }
            return setting;
        });
      }
      
}