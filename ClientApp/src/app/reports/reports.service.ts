import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import { PopupService } from '../popup/popup.service';

@Injectable()
export class ReportsService {

  constructor(
    @Inject("BASE_URL") private baseurl: string,
    private httpClient: HttpClient,
    private popupService: PopupService
  ) { }

  public getReports(): Observable<Report[]> {
    return this.popupService.addWaitDialog(
      this.httpClient.get<Report[]>(this.baseurl + 'api/v1/reports')
    );
  }

  public getReport(id: number): Observable<Report> {
    return this.popupService.addWaitDialog(
      this.httpClient.get<Report>(this.baseurl + 'api/v1/reports/' + id.toString())
    );
  }

  public createReport(report: Report): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.post(this.baseurl + 'api/v1/reports', report)
      .map(res => true, () => false)
    );
  }

  public editReport(report: Report): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.put(this.baseurl + 'api/v1/reports/' + report.id.toString(), report)
      .map(res => true, () => false)
    );
  }

  public deleteReport(id: number): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.delete(this.baseurl + 'api/v1/reports/' + id.toString())
      .map(res => true, () => false)
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
