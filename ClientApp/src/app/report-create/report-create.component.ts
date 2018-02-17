import { Component } from '@angular/core';

import { Report, ReportsService } from '../reports.service';

@Component({
  selector: 'app-report-create',
  templateUrl: './report-create.component.html',
  styleUrls: ['./report-create.component.css']
})
export class ReportCreateComponent {
  public report: Report;

  constructor(private service: ReportsService) {
    this.report = {
      id: 0,
      title: "",
      author: "",
      content: "",
      created: new Date(0),
      lastmodified: new Date(0),
      published: null
    };
  }

  submit() {
    this.service.createReport(this.report)
      .subscribe(ok => alert("Created"), error => alert("Failed"));
  }
}
