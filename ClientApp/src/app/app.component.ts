import { Component, ViewContainerRef } from '@angular/core';
import { PopupService } from './popup/popup.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styles: ["@media (max-width: 767px) { /* On small screens, the nav menu spans the full width of the screen. Leave a space for it. */    .body - content {    padding-top: 50px;  }}"]
  //styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  constructor(
    popupService: PopupService,
    ref: ViewContainerRef
  ) {
    popupService.setRoot(ref);
  }
}
