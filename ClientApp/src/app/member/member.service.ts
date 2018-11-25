import { Inject, Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { AuthHttp } from "angular2-jwt";

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { Report } from "../reports/reports.service";
import { PopupService } from '../popup/popup.service';

@Injectable()
export class MemberService {

  constructor(
    @Inject("BASE_URL") private baseurl: string,
    private http: AuthHttp,
    private popupService: PopupService
  ) { }

  public getMembers(): Observable<Member[]> {
    return this.popupService.addWaitDialog(
      this.http.get(this.baseurl + 'api/v1/members')
        .map(response => response.json() as Member[])
    );
  }

  public getMember(id: number): Observable<Member> {
    return this.popupService.addWaitDialog(
      this.http.get(this.baseurl + 'api/v1/members/' + id.toString())
        .map(response => response.json() as Member)
    );
  }

  public createMember(member: Member): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.http.post(this.baseurl + 'api/v1/members', member)
        .map(response => response.ok)
    );
  }

  public editMember(member: Member): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.http.put(this.baseurl + 'api/v1/members/' + member.id.toString(), member)
        .map(response => response.ok)
    );
  }

  public createReport(id: number, report: Report) {
    return this.popupService.addWaitDialog(
      this.http.post(this.baseurl + "api/v1/members/" + id + "/reports", report)
        .map(response => response.ok)
    );
  }

  public deleteMember(id: number): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.http.delete(this.baseurl + 'api/v1/members/' + id.toString())
        .map(response => response.ok)
    );
  }
}


export class Member {
  id: number;
  firstName: string;
  surname: string;
  alias: string;
  email: string;
  dateOfBirth: Date;
  position: string;
  rank: string;
  joined: Date;
  invested: Date;
  left: Date;
}
