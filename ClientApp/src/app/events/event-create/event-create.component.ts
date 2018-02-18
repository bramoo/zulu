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
    this.event = {
      id: 0,
      name: "",
      start: new Date(Date.now()),
      end: new Date(Date.now()),
      allDay: false,
      deleted: false
    };
  }

  submit() {
    this.service.createEvent(this.event)
      .subscribe(ok => alert("Created"), error => alert("Failed"));
  }
}
