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

    public settings: Settings[];

    constructor(private systemParametersService: SystemParametersService){
        
    }

    ngAfterViewInit(): void {
        this.listSettings();
    }

    listSettings(){
        this.settings = [];
        this.systemParametersService.listParameters().subscribe((settings: any) =>{
            for (let setting of settings){
                console.log(settings);
                this.settings.push(<Settings>settings);
            }
        });

    }
}