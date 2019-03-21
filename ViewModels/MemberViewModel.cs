using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using zulu.Data;

namespace zulu.ViewModels
{
  public class MemberViewModel : EntityViewModel
  {
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Alias { get; set; }
    public string Email { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string Position { get; set; }
    public string Rank { get; set; }

    public DateTime? Joined { get; set; }
    public DateTime? Invested { get; set; }
    public DateTime? Left { get; set; }

    public ImageDescriptionViewModel Mugshot { get; set; }
  }


  public class FullMemberViewModel : MemberViewModel
  {
    public IEnumerable<ImageDescriptionViewModel> Mugshots { get; set; }
  }


  public class MemberViewModelValidator : AbstractValidator<MemberViewModel>
  {
    public MemberViewModelValidator(AppDbContext dbContext)
    {
      if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
      RuleFor(m => m.FirstName).NotEmpty();
      RuleFor(m => m.Surname).NotEmpty();
      RuleFor(m => m.Email).EmailAddress()
        .Must((model, email) => !dbContext.Members.Any(m => m.Email == email && m.Id != model.Id)).WithMessage(m => $"A member with email '{m.Email}' already exists.");
      RuleFor(m => m.Joined).NotEmpty();
      RuleFor(m => m.Invested)
        .GreaterThanOrEqualTo(m => m.Joined).WithMessage("Can't be inversted before you joined.");
      RuleFor(m => m.Left)
        .GreaterThan(m => m.Joined).WithMessage("Can't leave before joining.")
        .GreaterThan(m => m.Invested).WithMessage("Can't leave before investiture.");
    }
  }


  public class FullMemberViewModelValidator : AbstractValidator<FullMemberViewModel>
  {
    public FullMemberViewModelValidator(AppDbContext dbContext)
    {
      if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
      RuleFor(m => m.FirstName).NotEmpty();
      RuleFor(m => m.Surname).NotEmpty();
      RuleFor(m => m.Email).EmailAddress()
        .Must((model, email) => !dbContext.Members.Any(m => m.Email == email && m.Id != model.Id)).WithMessage(m => $"A member with email '{m.Email}' already exists.");
      RuleFor(m => m.Joined).NotEmpty();
      RuleFor(m => m.Invested)
        .GreaterThanOrEqualTo(m => m.Joined).WithMessage("Can't be inversted before you joined.");
      RuleFor(m => m.Left)
        .GreaterThan(m => m.Joined).WithMessage("Can't leave before joining.")
        .GreaterThan(m => m.Invested).WithMessage("Can't leave before investiture.");
      
      RuleForEach(m => m.Mugshots).SetValidator(new ImageDescriptionViewModelValidator());
    }
  }
}