import { Inject, Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { AuthHttp } from "angular2-jwt";

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { Report } from "../reports/reports.service";
import { PopupService } from '../popup/popup.service';

@Injectable()
export class EventsService {

  constructor(
    @Inject("BASE_URL") private baseurl: string,
    private http: AuthHttp,
    private popupService: PopupService
  ) { }

  public getEvents(): Observable<Event[]> {
    let popupRef = this.popupService.waitDialog();

    return this.http.get(this.baseurl + 'api/v1/events')
      .map(response => response.json() as Event[]);
  }

  public getEvent(id: number): Observable<Event> {
    return this.http.get(this.baseurl + 'api/v1/events/' + id.toString())
      .map(response => response.json() as Event);
  }

  public createEvent(event: Event): Observable<boolean> {
    return this.http.post(this.baseurl + 'api/v1/events', event)
      .map(response => response.ok);
  }

  public editEvent(event: Event): Observable<boolean> {
    return this.http.put(this.baseurl + 'api/v1/events/' + event.id.toString(), event)
      .map(response => response.ok);
  }

  public createReport(id: number, report: Report) {
    return this.http.post(this.baseurl + "api/v1/events/" + id + "/reports", report)
      .map(response => response.ok);
  }

  public deleteEvent(id: number): Observable<boolean> {
    return this.http.delete(this.baseurl + 'api/v1/events/' + id.toString())
      .map(response => response.ok);
  }

  public deleteImage(id: any): Observable<boolean> {
    return this.http.delete(this.baseurl + 'api/v1/images/' + id)
      .map(response => response.ok);
  }
}


export class Event {
  id: number;
  name: string;
  start: Date;
  end: Date;
  allDay: boolean;
  deleted: false;
  images: any[];
}
