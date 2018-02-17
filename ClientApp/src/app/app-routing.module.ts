import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';

import { FacebookLoginComponent } from './facebook-login/facebook-login.component';
import { HomeComponent } from './home/home.component';
import { ReportCreateComponent } from './report-create/report-create.component';
import { ReportDetailsComponent } from './report-details/report-details.component';
import { ReportEditComponent } from './report-edit/report-edit.component';
import { ReportListComponent } from './report-list/report-list.component';

const routes: Route[] = [
  { path: '', component: HomeComponent },
  { path: 'login', component: FacebookLoginComponent },
  { path: 'reports', component: ReportListComponent },
  { path: 'reports/create', component: ReportCreateComponent },
  { path: 'reports/:reportid', component: ReportDetailsComponent },
  { path: 'reports/:reportid/edit', component: ReportEditComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
