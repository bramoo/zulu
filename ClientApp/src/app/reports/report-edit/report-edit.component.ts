import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Report, ReportsService } from '../reports.service';

@Component({
  selector: 'app-report-edit',
  templateUrl: './report-edit.component.html',
  styleUrls: ['./report-edit.component.css']
})
export class ReportEditComponent implements OnInit {
  public report: Report;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ReportsService
  ) { }

  ngOnInit() {
    this.route.paramMap
      .map(params => +params.get("reportid"))
      .switchMap(id => this.service.getReport(id))
      .subscribe(report => this.report = report);
  }

  submit() {
    this.service.editReport(this.report)
      .subscribe(ok => this.router.navigate(['/reports', this.report.id]));
  }
}
