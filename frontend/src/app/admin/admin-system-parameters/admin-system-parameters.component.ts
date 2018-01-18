import {Component, EventEmitter, Output, AfterViewInit} from '@angular/core';
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
    public model: Settings = <Settings>{};
    public errorMessage: string;
    

    constructor(private systemParametersService: SystemParametersService, private translate: TranslateService, private toastr: ToastrService){
        
    }

    ngAfterViewInit(): void {
        this.listSettings();
    }

    listSettings(){
        this.settingslist = [];
        this.systemParametersService.listParameters().subscribe((settingslist: any) =>{
            for (let setting of settingslist){
                this.settingslist.push(<Settings>setting);
            }
        });
       
    }

    editSettings() {
        this.systemParametersService.editParameters(this.model.id, this.model.varName, this.model.value).subscribe(
            () => {
                this.toastr.success(
                    this.translate.instant('SystemParameters.Save'), '',
                    {positionClass: 'toast-bottom-right'}
                )               
            },
            error => {
                alert(this.errorMessage);
                console.log(this.model.id);
             
            });       
    }
}