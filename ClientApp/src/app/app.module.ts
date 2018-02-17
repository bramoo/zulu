import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// components
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { FacebookLoginComponent } from './facebook-login/facebook-login.component';
import { HomeComponent } from './home/home.component';
import { ReportListComponent } from './report-list/report-list.component';
import { ReportDetailsComponent } from './report-details/report-details.component';
import { ReportEditComponent } from './report-edit/report-edit.component';
import { ReportCreateComponent } from './report-create/report-create.component';

// services
import { ReportsService } from './reports.service';

import { AppRoutingModule } from './app-routing.module';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    ReportListComponent,
    ReportDetailsComponent,
    ReportEditComponent,
    ReportCreateComponent,
    FacebookLoginComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule
  ],
  providers: [
    ReportsService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
