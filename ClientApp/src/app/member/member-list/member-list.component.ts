import { Component, OnInit } from '@angular/core';

import { Member, MemberService } from '../member.service';

@Component({
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  public members: Member[];

  constructor(private service: MemberService) { }

  ngOnInit() {
    this.service.getMembers()
      .subscribe(members => this.members = members);
  }

  delete(member: Member) {
    this.service.deleteMember(member.id)
      .subscribe(ok => alert("Deleted"), error => alert("Failed"));
  }
}
