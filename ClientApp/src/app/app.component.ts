import { Component, ViewContainerRef } from '@angular/core';
import { PopupService } from './popup/popup.service';
import { AuthService } from './auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  // styles: ["@media (max-width: 767px) { /* On small screens, the nav menu spans the full width of the screen. Leave a space for it. */    .body - content {    padding-top: 50px;  }}"]
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';

  constructor(
    private popupService: PopupService,
    private ref: ViewContainerRef, 
    private auth: AuthService,
    private router: Router
  ) {
    popupService.setRoot(ref);
  }

  logout() {
    this.auth.logout();
    this.router.navigate(['/login']);
  }
}
