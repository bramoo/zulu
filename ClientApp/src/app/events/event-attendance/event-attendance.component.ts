import { Component, Input, OnInit } from "@angular/core";

import { Member, MemberService } from "../../member/member.service";

import { Attendance, EventsService } from "../events.service";


@Component({
  selector: "event-attendance",
  templateUrl: "./event-attendance.component.html"
})
export class EventAttendanceComponent implements OnInit {
  @Input() eventId: number;
  public records: any = new Array<Attendance>();

  @Input()
  public set attendance(attendances: Attendance[]) {
    for (let attendance of attendances) {
      let existing = this.records.find(record => record.member.id == attendance.member.id);

      if (existing) {
        existing.attended = attendance.attended;
        existing.serviceHours = attendance.serviceHours;
      }
      else {
        this.records.push(attendance);
      }
    }

    this.records.sort((a, b) => this.orderMember(a.member, b.member))
  }

  public set members(members: Member[]) {
    for (let member of members) {
      let existing = this.records.find(record => record.member.id == member.id);

      if (!existing) {
        this.records.push({
          member: member,
          attended: undefined,
          serviceHours: undefined
        });
      }
    }

    this.records.sort((a, b) => this.orderMember(a.member, b.member))
  }

  constructor(
    private eventsService: EventsService,
    private memberService: MemberService
  ) { }

  ngOnInit() {
    this.memberService.getMembers().subscribe(members => this.members = members)
  }

  orderMember(a: Member, b: Member): number {
    return (a.firstName + " " + a.surname).localeCompare(b.firstName + " " + b.surname);
  }

  updateRecord(record: Attendance) {
    if (this.eventId && record) {
      if (record.attended == null) {
        this.eventsService.deleteAttendance(this.eventId, record.member.id).subscribe();
      }
      else {
        this.eventsService.updateAttendance(this.eventId, record).subscribe();
      }
    }
  }
}
