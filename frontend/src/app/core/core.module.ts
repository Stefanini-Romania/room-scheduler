import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { RSHeader } from './rs-header/rs-header.component';
import { RSFooter } from './rs-footer/rs-footer.component';
import { HttpModule } from '@angular/http';

@NgModule({
  imports: [ CommonModule, FormsModule, HttpModule ],
  providers: [],
  declarations: [RSHeader, RSFooter],
  exports: [ RSHeader, RSFooter ],
})
export class CoreModule {}