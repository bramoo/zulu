import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/switchMap';

import { Report, ReportsService } from '../reports.service';

@Component({
  selector: 'app-report-details',
  templateUrl: './report-details.component.html',
  styleUrls: ['./report-details.component.css']
})
export class ReportDetailsComponent implements OnInit {
  public report: Report;

  constructor(
    private route: ActivatedRoute,
    private service: ReportsService
  ) { }

  ngOnInit() {
    this.route.paramMap
      .map(params => +params.get("reportid"))
      .switchMap(id => this.service.getReport(id))
      .subscribe(report => this.report = report);
  }

}
