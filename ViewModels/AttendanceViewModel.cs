using FluentValidation;
using System;

namespace zulu.ViewModels
{
  public abstract class AttendanceViewModel
  {
    public bool Attended{get;set;}
    public decimal? ServiceHours { get; set; }
  }


  public class EventAttendanceViewModel : AttendanceViewModel
  {
    public MemberViewModel Member { get; set; }
  }


  public class MemberAttendanceViewModel : AttendanceViewModel
  {
    public EventViewModel Event { get; set; }
  }


  public class EventAttendanceViewModelValidator : AbstractValidator<EventAttendanceViewModel>
  {
    public EventAttendanceViewModelValidator()
    {
      RuleFor(a => a.Member).NotEmpty();
      RuleFor(a => a.Member.Id).NotEmpty();
      RuleFor(a => a.Attended).NotEmpty();
      RuleFor(a => a.ServiceHours).GreaterThanOrEqualTo(0);
    }
  }


  public class MemberAttendanceViewModelValidator : AbstractValidator<MemberAttendanceViewModel>
  {
    public MemberAttendanceViewModelValidator()
    {
      RuleFor(a => a.Event).NotEmpty();
      RuleFor(a => a.Event.Id).NotEmpty();
      RuleFor(a => a.Attended).NotEmpty();
      RuleFor(a => a.ServiceHours).GreaterThanOrEqualTo(0);
    }
  }
}