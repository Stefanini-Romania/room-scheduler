import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { RSHeader } from './rs-header/rs-header.component';
import { RSFooter } from './rs-footer/rs-footer.component';
import { HttpModule } from '@angular/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { LoginComponent } from '../auth/login/login.component';

@NgModule({
  imports: [ CommonModule, FormsModule, HttpModule, NgbModule.forRoot() ],
  providers: [],
  declarations: [RSHeader, RSFooter],
  exports: [ CommonModule, FormsModule, NgbModule, RSHeader, RSFooter ],
})
export class CoreModule {}