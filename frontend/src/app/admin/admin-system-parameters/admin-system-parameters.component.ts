import {Component, EventEmitter, Output, AfterViewInit} from '@angular/core';

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

    constructor(private systemParametersService: SystemParametersService){
        
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
}