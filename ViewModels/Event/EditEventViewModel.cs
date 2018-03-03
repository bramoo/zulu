using AutoMapper;
using FluentValidation;
using System;

namespace zulu.ViewModels.Event
{
	public class EditEventViewModel
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public bool AllDay { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
  }


  public class EditEventViewModelValidator : AbstractValidator<EditEventViewModel>
  {
    public EditEventViewModelValidator()
    {
      RuleFor(e => e.Name).NotEmpty();
      RuleFor(e => e.Start).NotEmpty();
      RuleFor(e => e.End).NotEmpty().GreaterThan(e => e.Start).WithMessage("'End' must be greater than 'Start'");
    }
  }


  public class EditEventViewModelProfile : Profile
  {
    public EditEventViewModelProfile()
    {
      CreateMap<EditEventViewModel, zulu.Models.Event>()
          .ForMember(e => e.Id, opt => opt.Ignore());
    }
  }
}