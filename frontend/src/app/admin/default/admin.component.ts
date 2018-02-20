
import {Component, EventEmitter, Output, ElementRef, AfterViewInit} from '@angular/core';
import {Router} from '@angular/router';
import {TranslateService} from "@ngx-translate/core";

import {RoleEnum} from '../../shared/models/role.model';
import {AuthService} from '../../auth/shared/auth.service';
import {AdminUsersTab} from '../admin-users-tab/admin-users-tab.component';
import {AdminRoomsTab} from '../admin-rooms-tab/admin-rooms-tab.component';
import {AdminSystemParameters} from '../admin-system-parameters/admin-system-parameters.component';
//import {HostAvailability} from './../../shared/hosts/host-availability.component';

@Component({
    selector: 'admin-component',
    templateUrl: './admin.component.html',
    styleUrls: [],
    providers: [],
})


export class AdminComponent implements AfterViewInit{
    public isHost: boolean;

    constructor(private translate: TranslateService, private router: Router, private authService: AuthService) {
        
    }
 
    ngAfterViewInit(): void {
        if (!this.authService.isLoggedIn() || !this.authService.getLoggedUser().hasRole(RoleEnum.admin) || !this.authService.getLoggedUser().hasRole(RoleEnum.host)) {
            // this.router.navigate(['calendar']);
        }
    
        if(this.authService.getLoggedUser().hasRole(RoleEnum.admin)){
             this.isHost=false;
        }
        else if(this.authService.getLoggedUser().hasRole(RoleEnum.host)){
            this.isHost=true;
        }
    }

}
