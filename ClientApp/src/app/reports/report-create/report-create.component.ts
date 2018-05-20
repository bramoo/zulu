import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

import { Event, EventsService } from "../../events/events.service";
import { Report, ReportsService } from '../reports.service';

@Component({
  selector: 'app-report-create',
  templateUrl: './report-create.component.html',
  styleUrls: ['./report-create.component.css']
})
export class ReportCreateComponent {
  public event: number;
  public events: Event[];
  public report = new Report();

  constructor(
    private route: ActivatedRoute,
    private reportService: ReportsService,
    private eventService: EventsService
  ) { }

  ngOnInit() {
    let params = this.route.snapshot.paramMap;

    if (params.has("event")) {
      this.event = +params.get("event");
    }

    this.eventService.getEvents().subscribe(events => this.events = events);
  }

  submit() {
    if (this.event) {
      this.eventService.createReport(this.event, this.report)
        .subscribe(ok => alert("Created"), error => alert("Failed"));
    }
    else {
      this.reportService.createReport(this.report)
        .subscribe(ok => alert("Created"), error => alert("Failed"));
    }
  }
}
