using FluentValidation;
using System;
using System.Collections.Generic;

namespace zulu.ViewModels
{
  public class EventViewModel : EntityViewModel
  {
    public string Name { get; set; }
    public bool AllDay { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
  }


  public class FullEventViewModel : EventViewModel
  {
    // public IEnumerable<AttendanceViewModel> Attendance { get; set; }
    public IEnumerable<ReportViewModel> Reports { get; set; }
    public IEnumerable<ImageDescriptionViewModel> Images { get; set; }
  }


  public class EventViewModelValidator : AbstractValidator<EventViewModel>
  {
    public EventViewModelValidator()
    {
      RuleFor(e => e.Name).NotEmpty();
      RuleFor(e => e.Start).NotEmpty();
      RuleFor(e => e.End).NotEmpty().GreaterThan(e => e.Start).WithMessage("'End' must be after 'Start'");
    }
  }

  public class FullEventViewModelValidator : AbstractValidator<FullEventViewModel>
  {
    public FullEventViewModelValidator()
    {
      RuleFor(e => e.Name).NotEmpty();
      RuleFor(e => e.Start).NotEmpty();
      RuleFor(e => e.End).NotEmpty().GreaterThan(e => e.Start).WithMessage("'End' must be after 'Start'");

      // RuleFor(e => e.Attendance).SetCollectionValidator(new AttendanceViewModelValidator());
      RuleFor(e => e.Reports).SetCollectionValidator(new ReportViewModelValidator());
      RuleFor(e => e.Images).SetCollectionValidator(new ImageDescriptionViewModelValidator());
    }
  }
}