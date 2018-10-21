import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { PopupService } from './popup.service';
import { PopupComponent } from './popup.component';


@NgModule({
  imports: [
    CommonModule
  ],

  declarations: [
    PopupComponent
  ],

  entryComponents: [
    PopupComponent
  ],

  providers: [
    PopupService
  ]
})
export class PopupModule { }
