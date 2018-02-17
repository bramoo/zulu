import { Component, OnInit } from '@angular/core';

import { Report, ReportsService } from '../reports.service';

@Component({
  selector: 'app-report-list',
  templateUrl: './report-list.component.html',
  styleUrls: ['./report-list.component.css']
})
export class ReportListComponent implements OnInit {
  public reports: Report[];

  constructor(private service: ReportsService) { }

  ngOnInit() {
    this.service.getReports()
      .subscribe(reports => this.reports = reports);
  }

  delete(report: Report) {
    this.service.deleteReport(report.id)
      .subscribe(ok => alert("Deleted"), error => alert("Failed"));
  }
}
