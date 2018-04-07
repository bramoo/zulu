import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styles: ["@media (max-width: 767px) { /* On small screens, the nav menu spans the full width of the screen. Leave a space for it. */    .body - content {    padding-top: 50px;  }}"]
  //styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
}
