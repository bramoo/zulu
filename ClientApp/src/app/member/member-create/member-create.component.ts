import { Component } from '@angular/core';

import { Member, MemberService } from '../member.service';

@Component({
  templateUrl: './member-create.component.html',
  styleUrls: ['./member-create.component.css']
})
export class MemberCreateComponent {
  public member = new Member();

  constructor(private service: MemberService) { }

  submit() {
    this.service.createMember(this.member)
      .subscribe(ok => alert("Created"), error => alert("Failed"));
  }
}
