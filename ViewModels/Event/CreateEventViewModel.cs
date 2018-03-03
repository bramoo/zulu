using AutoMapper;
using FluentValidation;
using System;

namespace zulu.ViewModels.Event
{
	public class CreateEventViewModel
  {
    public string Name { get; set; }
    public bool AllDay { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
  }


  public class CreateEventViewModelValidator : AbstractValidator<CreateEventViewModel>
  {
    public CreateEventViewModelValidator()
    {
      RuleFor(e => e.Name).NotEmpty();
      RuleFor(e => e.Start).NotEmpty();
      RuleFor(e => e.End).NotEmpty().GreaterThan(e => e.Start).WithMessage("'End' must be greater than 'Start'");
    }
  }


  public class CreateEventViewModelProfile : Profile
  {
    public CreateEventViewModelProfile()
    {
      CreateMap<CreateEventViewModel, zulu.Models.Event>();
    }
  }
}