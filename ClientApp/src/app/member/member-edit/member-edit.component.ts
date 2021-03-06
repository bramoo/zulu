import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { Member, MemberService } from '../member.service';

@Component({
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  public member: Member;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: MemberService
  ) { }

  ngOnInit() {
    this.route.paramMap
      .map(params => +params.get("memberid"))
      .switchMap(id => this.service.getMember(id))
      .subscribe(member => this.member = member);
  }

  submit() {
    this.service.editMember(this.member)
      .subscribe(ok => this.router.navigate(['/members', this.member.id]));
  }
}
