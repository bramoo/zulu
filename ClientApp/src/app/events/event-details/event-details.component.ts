import { Component, Inject, OnInit } from '@angular/core';
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
    @Inject("BASE_URL") private baseurl: string,
    private route: ActivatedRoute,
    private service: EventsService
  ) { }

  ngOnInit() {
    this.route.paramMap
      .map(params => +params.get("eventid"))
      .switchMap(id => this.service.getEvent(id))
      .subscribe(event => this.event = event);
  }

  public delete(image: any) {
    this.service.deleteImage(image.id).subscribe(ok => {
      if (ok) this.ngOnInit();
    });
  }

  public imageUrl(id: any) {
    return this.baseurl + 'api/v1/images/' + id;
  }

  public imageUploaded() {
    this.ngOnInit();
  }
}
