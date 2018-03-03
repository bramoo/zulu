import { Inject, Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { AuthHttp } from "angular2-jwt";

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class EventsService {

  constructor(
    @Inject("BASE_URL") private baseurl: string,
    private http: AuthHttp
  ) { }

  public getEvents(): Observable<Event[]> {
    return this.http.get(this.baseurl + 'api/v1/events')
      .map(response => response.json() as Event[]);
  }

  public getEvent(id: number): Observable<Event> {
    return this.http.get(this.baseurl + 'api/v1/events/' + id.toString())
      .map(response => response.json() as Event);
  }

  public createEvent(report: Event): Observable<boolean> {
    return this.http.post(this.baseurl + 'api/v1/events', report)
      .map(response => response.ok);
  }

  public editEvent(report: Event): Observable<boolean> {
    return this.http.put(this.baseurl + 'api/v1/events/' + report.id.toString(), report)
      .map(response => response.ok);
  }

  public deleteEvent(id: number): Observable<boolean> {
    return this.http.delete(this.baseurl + 'api/v1/events/' + id.toString())
      .map(response => response.ok);
  }
}


export interface Event {
  id: number;
  name: string;
  start: Date;
  end: Date;
  allDay: boolean;
  deleted: false;
}
