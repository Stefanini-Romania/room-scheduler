import {Component, EventEmitter, Output, OnInit, AfterViewInit} from '@angular/core';

@Component({
    selector: 'admin-host-tab',
    templateUrl: './admin-host-tab.component.html',
    styleUrls: [],
    
})

export class AdminHostComponent {
    public currentYear;
    public weeks =[];

    ngAfterViewInit(){  
        this.getCurrentYear();  
        this.getWeeksInYear((this.currentYear));
    //    console.log(this.getWeeksInYear(this.currentYear));
    }

    constructor(){
        
    } 
    getCurrentYear(){
        let today = new Date();
        this.currentYear = today.getFullYear();
        return this.currentYear;
    }

     getWeeksInYear(year){
        this.weeks=[]
        var firstDate=new Date(year, 0, 1),
            lastDate=new Date(year, 11, 31), 
            numDays= lastDate.getDate();
        
        var start=1;
        var end=7-firstDate.getDay();

        while(start<=365){
            
            this.weeks.push({start:start,end:end});
            start = end + 1;
            end = end + 7;
            end = start === 1 && end === 8 ? 1 : end;
             if(end>365)
              end=365;    
        }      

        
         return this.weeks;
     }  
     
      
}