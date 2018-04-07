using AutoMapper;
using FluentValidation;
using System;
using System.Linq;
using System.Collections.Generic;
using zulu.Data;
using zulu.ViewModels.ValueResolvers;

namespace zulu.ViewModels.Member
{
  public class CreateMemberViewModel
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
  }


  public class CreateMemberViewModelValidator : AbstractValidator<CreateMemberViewModel>
  {
    public CreateMemberViewModelValidator(AppDbContext dbContext)
    {
      if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));

      RuleFor(m => m.FirstName).NotEmpty();
      RuleFor(m => m.Surname).NotEmpty();
      RuleFor(m => m.Email).EmailAddress()
        .Must(email => !dbContext.Members.Any(m => m.Email == email)).WithMessage(m => $"A member with email '{m.Email}' already exists.");
      RuleFor(m => m.Joined).NotEmpty();
      RuleFor(m => m.Invested)
        .GreaterThanOrEqualTo(m => m.Joined).WithMessage("Can't be inversted before you joined.");
      RuleFor(m => m.Left)
        .GreaterThan(m => m.Joined).WithMessage("Can't leave after joining.")
        .GreaterThan(m => m.Invested).WithMessage("Can't leave before investiture.");
    }
  }


  public class CreateMemberViewModelProfile : Profile
  {
    public CreateMemberViewModelProfile()
    {
      CreateMap<CreateMemberViewModel, zulu.Models.Member>()
        .ForMember(vm => vm.Position, opt => opt.ResolveUsing<CreateMemberValueResolver>())
        .ForMember(vm => vm.Rank, opt => opt.ResolveUsing<CreateMemberValueResolver>());
    }
  }
}