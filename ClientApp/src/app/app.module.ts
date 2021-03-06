// angular
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, ErrorHandler } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

// external
import { MarkdownModule } from 'ngx-markdown';
import { SimplemdeModule, SIMPLEMDE_CONFIG } from 'ng2-simplemde'

// components
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

// modules
import { AuthModule } from './auth/auth.module';
import { EventsModule } from './events/events.module';
import { MemberModule } from './member/member.module';
import { PopupModule } from './popup/popup.module';
import { ReportsModule } from './reports/reports.module';

import { AppRoutingModule } from './app-routing.module';

// util
import { PopupErrorHandler } from './error-handler/error-handler';
import { AuthInterceptor } from './auth/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpModule,
    HttpClientModule,
    FormsModule,
    MarkdownModule.forRoot(),
    SimplemdeModule.forRoot({
      provide: SIMPLEMDE_CONFIG,
      useValue: {placeholder: "Report goes here"}
    }),
    AuthModule,
    EventsModule,
    MemberModule,
    PopupModule,
    ReportsModule,
    AppRoutingModule
  ],
  providers: [
    { provide: ErrorHandler, useClass: PopupErrorHandler },
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
