import {Component, EventEmitter, Output, OnInit, AfterViewInit, ViewChild, ElementRef} from '@angular/core';
import {TranslateService, LangChangeEvent} from "@ngx-translate/core";
import {ToastrService} from 'ngx-toastr';
import { Subscription } from 'rxjs/Subscription';
import {NgbDropdownConfig} from '@ng-bootstrap/ng-bootstrap';

import {HostService} from './../../services/host.service';
import {User} from './../../models/user.model';


@Component({
    selector: 'host-selector',
    templateUrl: './host-selector.component.html',
    styleUrls: [],
    providers: []
    
})

export class HostSelector {
    @ViewChild('location') input: ElementRef;

    public hosts: User[] = [];
    public selectedHost: User;
    public filteredHosts: User[] = [];
    public lastName: string;
    public selectedHostName: string;
    private subscription: Subscription;
    
    constructor(config: NgbDropdownConfig, private HostService: HostService, translate: TranslateService){
        this.subscription = translate.onLangChange.subscribe((event: LangChangeEvent) => {
            this.selectedHostName = this.selectedHost.firstName.concat(this.selectedHost.lastName);
        });
    }

    ngAfterViewInit(){
        this.hosts = [];
        this.HostService.HostList().subscribe((hosts: any) => {
            for (let host of hosts) {
                this.hosts.push(<User>host);
            }

            this.onSelectHost(hosts[0]);
            this.filteredHostsByName();
        });
    }

    focusOnName() {
        if (this.input) {
            setTimeout(()=> {
                this.input.nativeElement.focus();
            });
        }
    }

    onSelectHost(host: User) {
        this.selectedHost = host;

        // broadcast global event that host has changed
        this.HostService.selectHost(host);
    }

    private assignCopy(){
        this.filteredHosts = Object.assign([], this.hosts);
        return this.filteredHosts;
    }

    filteredHostsByName(lastName: string = '') {
        if(!lastName) {
            //when nothing has typed
            this.assignCopy();
        }

        this.filteredHosts = this.assignCopy().filter(
            (host:User) => {
                return host.lastName.toLowerCase().indexOf(lastName.toLowerCase()) > -1
            }
        );
    }
}