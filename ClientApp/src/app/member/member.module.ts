import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Route, RouterModule } from '@angular/router';
import { AuthGuard } from "../auth/auth.guard";

import { MemberListComponent } from './member-list/member-list.component';
import { MemberCreateComponent } from './member-create/member-create.component';
import { MemberEditComponent } from './member-edit/member-edit.component';
import { MemberDetailsComponent } from './member-details/member-details.component';

import { MemberService } from './member.service';

const memberRoutes: Route[] = [
  {
    path: 'members', canActivate: [AuthGuard], children: [
      { path: '', component: MemberListComponent },
      { path: 'create', component: MemberCreateComponent },
      { path: ':memberid', component: MemberDetailsComponent },
      { path: ':memberid/edit', component: MemberEditComponent }
    ]
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(memberRoutes)
  ],
  exports: [
    RouterModule
  ],
  declarations: [
    MemberListComponent,
    MemberCreateComponent,
    MemberEditComponent,
    MemberDetailsComponent
  ],
  providers: [
    MemberService
  ]
})
export class MemberModule { }
