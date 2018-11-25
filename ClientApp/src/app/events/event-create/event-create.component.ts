import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { Event, EventsService } from '../events.service';

@Component({
  selector: 'app-event-create',
  templateUrl: './event-create.component.html',
  styleUrls: ['./event-create.component.css']
})
export class EventCreateComponent implements OnInit {
  public event = new Event();

  constructor(
    private service: EventsService,
    private router: Router
  ) { }

  ngOnInit() {
    this.event.start = new Date(Date.now());
    this.event.end = new Date(Date.now());
    this.event.reports = [];
    this.event.images = [];
  }

  submit() {
    this.service.createEvent(this.event)
      .subscribe(ok => this.router.navigate(['/events']));
  }
}
