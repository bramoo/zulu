import { Component } from '@angular/core';

import { Member, MemberService } from '../member.service';
import { Router } from '@angular/router';

@Component({
  templateUrl: './member-create.component.html',
  styleUrls: ['./member-create.component.css']
})
export class MemberCreateComponent {
  public member = new Member();

  constructor(
    private router: Router,
    private service: MemberService
  ) { }

  submit() {
    this.service.createMember(this.member)
      .subscribe(ok => this.router.navigate(['/members']));
  }
}
