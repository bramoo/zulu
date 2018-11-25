using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using zulu.Data;
using zulu.Models;

namespace zulu.ViewModels
{
  public abstract class AssignmentViewModel<TAssignment> : EntityViewModel where TAssignment : Assignment
  {
    public string AssignmentType { get; set; }
    public string Description { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? CompletionDate { get; set; }

    public MemberViewModel Owner { get; set; }
    public MemberViewModel Assignee { get; set; }
    public ISet<MemberViewModel> Followers { get; set; }
  }

  public abstract class AssignmentViewModelValidator<TAssignment, TViewModel> : AbstractValidator<TViewModel> 
    where TAssignment : Assignment
    where TViewModel : AssignmentViewModel<TAssignment>
  {
    public AssignmentViewModelValidator(AppDbContext dbContext)
    {
      if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
      RuleFor(a => a.AssignmentType).NotEmpty();
      RuleFor(a => a.Description).NotEmpty();
      RuleFor(a => a.Owner).Must((model, owner) => dbContext.Members.Any(m => owner.Id == m.Id)).WithMessage((a, m) => $"No member with id '{m.Id}' exists.");
      RuleFor(a => a.Assignee).Must((model, assignee) => dbContext.Members.Any(m => assignee.Id == m.Id)).WithMessage((a, m) => $"No member with id '{m.Id}' exists.");
      RuleForEach(a => a.Followers).Must((model, follower) => dbContext.Members.Any(m => follower.Id == m.Id)).WithMessage((a, m) => $"No member with id '{m.Id}' exists.");
    }
  }
}