import { Inject, Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { AuthHttp } from "angular2-jwt";

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { PopupService } from '../popup/popup.service';

@Injectable()
export class ReportsService {

  constructor(
    @Inject("BASE_URL") private baseurl: string,
    private http: AuthHttp,
    private popupService: PopupService
  ) { }

  public getReports(): Observable<Report[]> {
    return this.popupService.addWaitDialog(
      this.http.get(this.baseurl + 'api/v1/reports')
        .map(response => response.json() as Report[])
    );
  }

  public getReport(id: number): Observable<Report> {
    return this.popupService.addWaitDialog(
      this.http.get(this.baseurl + 'api/v1/reports/' + id.toString())
        .map(response => response.json() as Report)
    );
  }

  public createReport(report: Report): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.http.post(this.baseurl + 'api/v1/reports', report)
        .map(response => response.ok)
    );
  }

  public editReport(report: Report): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.http.put(this.baseurl + 'api/v1/reports/' + report.id.toString(), report)
        .map(response => response.ok)
    );
  }

  public deleteReport(id: number): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.http.delete(this.baseurl + 'api/v1/reports/' + id.toString())
        .map(response => response.ok)
    );
  }
}


export class Report {
  id: number;
  title: string;
  content: string;
  author: string;
  created: Date;
  modified: Date;
  state: number;
}
