import { Inject, Injectable, ComponentRef } from '@angular/core';
import { Http, Response } from '@angular/http';
import { AuthHttp } from "angular2-jwt";

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { Member } from "../member/member.service";

import { Report } from "../reports/reports.service";
import { PopupService } from '../popup/popup.service';
import { PopupComponent } from '../popup/popup.component';

@Injectable()
export class EventsService {

  constructor(
    @Inject("BASE_URL") private baseurl: string,
    private http: AuthHttp,
    private popupService: PopupService
  ) { }

  public getEvents(): Observable<Event[]> {
    return this.popupService.addWaitDialog(
      this.http.get(this.baseurl + 'api/v1/events')
        .map(response => response.json() as Event[])
    );
  }

  public getEvent(id: number): Observable<Event> {
    return this.popupService.addWaitDialog(
      this.http.get(this.baseurl + 'api/v1/events/' + id.toString())
        .map(response => response.json() as Event)
    );
  }

  public createEvent(event: Event): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.http.post(this.baseurl + 'api/v1/events', event)
        .map(response => response.ok)
    );
  }

  public editEvent(event: Event): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.http.put(this.baseurl + 'api/v1/events/' + event.id.toString(), event)
        .map(response => response.ok)
    );
  }

  public createReport(id: number, report: Report) {
    return this.popupService.addWaitDialog(
      this.http.post(this.baseurl + "api/v1/events/" + id + "/reports", report)
        .map(response => response.ok)
    );
  }

  public deleteEvent(id: number): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.http.delete(this.baseurl + 'api/v1/events/' + id.toString())
        .map(response => response.ok)
    );
  }

  public deleteImage(id: any): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.http.delete(this.baseurl + 'api/v1/images/' + id)
        .map(response => response.ok)
    );
  }

  public updateAttendance(id: number, attendance: Attendance) {
    return this.http.post(this.baseurl + 'api/v1/events/' + id + '/attendance', attendance);
  }

  public deleteAttendance(eventId: number, memberId: number) {
    return this.http.delete(this.baseurl + 'api/v1/events/' + eventId + '/attendance/' + memberId);
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
  reports: any[];
  attendance: Attendance[];
}

export class Attendance {
  member: Member;
  attended: boolean;
  serviceHours: number;
}
