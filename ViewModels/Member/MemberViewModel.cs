using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using zulu.ViewModels.ValueResolvers;

namespace zulu.ViewModels.Member
{
  public class MemberViewModel
  {
    public int Id { get; set; }
    public string State { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }

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


  public class MemberViewModelValidator : AbstractValidator<MemberViewModel>
  {
    public MemberViewModelValidator()
    {
      RuleFor(e => e.Id).NotEmpty();
      RuleFor(e => e.FirstName).NotEmpty();
      RuleFor(e => e.Surname).NotEmpty();
      RuleFor(e => e.Email).EmailAddress();
      RuleFor(e => e.Joined).NotEmpty();
      RuleFor(e => e.Invested)
        .GreaterThanOrEqualTo(m => m.Joined).WithMessage("Can't be inversted before you joined.");
      RuleFor(m => m.Left)
        .GreaterThan(m => m.Joined).WithMessage("Can't leave after joining.")
        .GreaterThan(m => m.Invested).WithMessage("Can't leave before investiture.");
    }
  }


  public class MemberViewModelProfile : Profile
  {
    public MemberViewModelProfile()
    {
      CreateMap<MemberViewModel, zulu.Models.Member>()
        .ForMember(vm => vm.Position, opt => opt.ResolveUsing<MemberPositionValueResolver>())
        .ForMember(vm => vm.Rank, opt => opt.ResolveUsing<MemberRankValueResolver>())
        .ReverseMap()
        .ForMember(m => m.Position, opt => opt.ResolveUsing<MemberPositionValueResolver>())
        .ForMember(m => m.Rank, opt => opt.ResolveUsing<MemberRankValueResolver>());
    }
  }
}