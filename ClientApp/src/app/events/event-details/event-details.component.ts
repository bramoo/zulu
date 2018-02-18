import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import 'rxjs/add/operator/filter';
import 'rxjs/add/operator/switchMap';

import { Event, EventsService } from '../events.service';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.css']
})
export class EventDetailsComponent implements OnInit {
  public event: Event;

  constructor(
    private route: ActivatedRoute,
    private service: EventsService
  ) { }

  ngOnInit() {
    this.route.paramMap
      .map(params => +params.get("eventid"))
      .switchMap(id => this.service.getEvent(id))
      .subscribe(event => this.event = event);
  }

}
