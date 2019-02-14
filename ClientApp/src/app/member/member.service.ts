import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

import { Report } from "../reports/reports.service";
import { PopupService } from '../popup/popup.service';

@Injectable()
export class MemberService {

  constructor(
    @Inject("BASE_URL") private baseurl: string,
    private httpClient: HttpClient,
    private popupService: PopupService
  ) { }

  public getMembers(): Observable<Member[]> {
    return this.popupService.addWaitDialog(
      this.httpClient.get<Member[]>(this.baseurl + 'api/v1/members')
    );
  }

  public getMember(id: number): Observable<Member> {
    return this.popupService.addWaitDialog(
      this.httpClient.get<Member>(this.baseurl + 'api/v1/members/' + id.toString())
    );
  }

  public createMember(member: Member): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.post(this.baseurl + 'api/v1/members', member)
        .map(() => true)
    );
  }

  public editMember(member: Member): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.put(this.baseurl + 'api/v1/members/' + member.id.toString(), member)
        .map(() => true)
    );
  }

  public createReport(id: number, report: Report) {
    return this.popupService.addWaitDialog(
      this.httpClient.post(this.baseurl + "api/v1/members/" + id + "/reports", report)
        .map(() => true)
    );
  }

  public deleteMember(id: number): Observable<boolean> {
    return this.popupService.addWaitDialog(
      this.httpClient.delete(this.baseurl + 'api/v1/members/' + id.toString())
        .map(() => true)
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

  get fullName() { return this.firstName + " " + this.surname }
}
