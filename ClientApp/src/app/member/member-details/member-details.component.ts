import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Member, MemberService } from '../member.service';

@Component({
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css']
})
export class MemberDetailsComponent implements OnInit {
  public member: Member;

  constructor(
    private route: ActivatedRoute,
    private service: MemberService
  ) { }

  ngOnInit() {
    this.route.paramMap
      .map(params => +params.get("memberid"))
      .switchMap(id => this.service.getMember(id))
      .subscribe(member => this.member = member);
  }
}
