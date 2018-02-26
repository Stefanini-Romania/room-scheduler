import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import {RsFooterComponent} from './rs-footer.component';

describe('RsFooterComponent (templateUrl)', ()=>{
    let component: RsFooterComponent;
    let fixture: ComponentFixture<RsFooterComponent>;
    let de: DebugElement;
    let el: HTMLElement;
   

    //async BeforeEach
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ ],
            declarations: [RsFooterComponent], // declare the test component
            providers: []
        })
        .compileComponents();  // compile template and css
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(RsFooterComponent);

        component = fixture.componentInstance; // RsFooterComponent test instance

        // query for the title <button>, a or div by CSS element selector
        de = fixture.debugElement.query(By.css('p'));
        el = de.nativeElement;
    });

    it('should display copyright', () => {
        fixture.detectChanges();
        expect(el.textContent).toContain('Stefanini');
    });

});