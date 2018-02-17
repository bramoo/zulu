import { Inject, Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

@Injectable()
export class ReportsService {

  constructor(
    @Inject("BASE_URL") private baseurl: string,
    private http: Http
  ) { }

  public getReports(): Observable<Report[]> {
    return this.http.get(this.baseurl + 'api/v1/reports')
      .map(response => response.json() as Report[]);
  }

  public getReport(id: number): Observable<Report> {
    return this.http.get(this.baseurl + 'api/v1/report/' + id.toString())
      .map(response => response.json() as Report);
  }

  public createReport(report: Report): Observable<boolean> {
    return this.http.post(this.baseurl + 'api/v1/report', report)
      .map(response => response.ok);
  }

  public editReport(report: Report): Observable<boolean> {
    return this.http.put(this.baseurl + 'api/v1/report/' + report.id.toString(), report)
      .map(response => response.ok);
  }

  public deleteReport(id: number): Observable<boolean> {
    return this.http.delete(this.baseurl + 'api/v1/report/' + id.toString())
      .map(response => response.ok);
  }
}


export interface Report {
  id: number;
  title: string;
  content: string;
  created: Date;
  lastmodified: Date;
  published: Date;
  author: string;
}
