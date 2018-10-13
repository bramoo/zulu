import { Component } from '@angular/core';

import { Event, EventsService } from '../events.service';

@Component({
  selector: 'app-event-create',
  templateUrl: './event-create.component.html',
  styleUrls: ['./event-create.component.css']
})
export class EventCreateComponent {
  public event: Event;

  constructor(private service: EventsService) {
    this.event = new Event();
    this.event.start = new Date(Date.now());
    this.event.end = new Date(Date.now());
  }

  submit() {
    this.service.createEvent(this.event)
      .subscribe(ok => alert("Created"), error => alert("Failed"));
  }
}
