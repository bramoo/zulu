import { Component, OnInit } from '@angular/core';

import { Event, EventsService } from '../events.service';

@Component({
  selector: 'app-event-list',
  templateUrl: './event-list.component.html',
  styleUrls: ['./event-list.component.css']
})
export class EventListComponent implements OnInit {
  public events: Event[];

  constructor(private service: EventsService) { }

  ngOnInit() {
    this.service.getEvents()
      .subscribe(events => this.events = events);
  }

  delete(event: Event) {
    this.service.deleteEvent(event.id)
      .subscribe(ok => this.ngOnInit());
  }
}
