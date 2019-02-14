using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using zulu.Data;
using zulu.Models;
using zulu.Models.Assignments;

namespace zulu.ViewModels.Assignments
{
  public class WriteEventReportAssignmentViewModel : AssignmentViewModel<WriteEventReportAssignment>
  {
    public EventViewModel Event { get; set; }
  }

  public class WriteEventReportAssignmentValidator : AssignmentViewModelValidator<WriteEventReportAssignment, WriteEventReportAssignmentViewModel>
  {
    public WriteEventReportAssignmentValidator(AppDbContext dbContext)
      : base(dbContext)
    {
      RuleFor(a => a.Event).Must((model, e) => dbContext.Events.Any(m => e.Id == m.Id)).WithMessage((a, e) => $"No event with id '{e.Id}' exists.");
    }
  }
}