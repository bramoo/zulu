import { Inject, Injectable, ComponentRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';


import { Observable } from 'rxjs/Observable';
import { map } from "rxjs/operators";

import { Member } from "../member/member.service";

import { Report } from "../reports/reports.service";
import { PopupService } from '../popup/popup.service';
import { PopupComponent } from '../popup/popup.component';

@Injectable()
export class EventsService {

  constructor(
    @Inject("BASE_URL") private baseurl: string,
    private httpClient: HttpClient,
    private popupService: PopupService
  ) { }

  public getEvents(): Observable<Event[]> {
    return this.popupService.addWaitDialog(
      this.httpClient.get<Event[]>("api/v1/events")
    );
  }

  public getEvent(id: number): Observable<Event> {
    return this.popupService.addWaitDialog(
      this.httpClient.get<Event>("api/v1/events/" + id.toString())
    );
  }

  public createEvent(event: Event): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.post("api/v1/events", event)
        .map(() => true)
    );
  }

  public editEvent(event: Event): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.put("/api/v1/events/" + event.id.toString(), event)
        .map(() => true)
    );
  }

  public createReport(id: number, report: Report): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.post("api/v1/events/" + id + "/reports", report)
        .map(() => true)
    );
  }

  public deleteEvent(id: number): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.delete('/api/v1/events/' + id.toString())
        .map(() => true)
    );
  }

  public deleteImage(id: any): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.delete('/api/v1/images/' + id)
        .map(() => true)
    );
  }

  public updateAttendance(id: number, attendance: Attendance) {
    return this.httpClient.post('/api/v1/events/' + id + '/attendance', attendance);
  }

  public deleteAttendance(eventId: number, memberId: number) {
    return this.httpClient.delete('api/v1/events/' + eventId + '/attendance/' + memberId);
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
