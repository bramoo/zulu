import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Route, RouterModule } from '@angular/router';

import { SimplemdeModule } from 'ng2-simplemde'

import { ReportCreateComponent } from './report-create/report-create.component';
import { ReportDetailsComponent } from './report-details/report-details.component';
import { ReportEditComponent } from './report-edit/report-edit.component';
import { ReportListComponent } from './report-list/report-list.component';

import { ReportsService } from './reports.service';

const reportRoutes: Route[] = [
  {
    path: 'reports', children: [
      { path: '', component: ReportListComponent },
      { path: 'create', component: ReportCreateComponent },
      { path: ':reportid', component: ReportDetailsComponent },
      { path: ':reportid/edit', component: ReportEditComponent}
    ]
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    SimplemdeModule,
    RouterModule.forChild(reportRoutes)
  ],
  exports: [
    RouterModule
  ],
  declarations: [
    ReportCreateComponent,
    ReportDetailsComponent,
    ReportEditComponent,
    ReportListComponent
  ],
  providers: [
    ReportsService
  ]
})
export class ReportsModule { }
