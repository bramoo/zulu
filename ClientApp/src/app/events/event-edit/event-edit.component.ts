import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Event, EventsService } from '../events.service';

@Component({
  selector: 'app-event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.css']
})
export class EventEditComponent implements OnInit {
  public event: Event;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: EventsService
  ) { }

  ngOnInit() {
    this.route.paramMap
      .map(params => +params.get("eventid"))
      .switchMap(id => this.service.getEvent(id))
      .subscribe(event => this.event = event);
  }

  submit() {
    this.service.editEvent(this.event)
      .subscribe(ok => this.router.navigate(['/events', this.event.id]));
  }
}
