using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using zulu.ViewModels.Report;

namespace zulu.ViewModels.Event
{
  public class EventViewModel
  {
    public int Id { get; set; }
    public string State { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }

    public string Name { get; set; }
    public bool AllDay { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public IEnumerable<ListReportViewModel> Reports { get; set; }
  }


  public class EventViewModelValidator : AbstractValidator<EventViewModel>
  {
    public EventViewModelValidator()
    {
      RuleFor(e => e.Id).NotEmpty();
      RuleFor(e => e.Name).NotEmpty();
      RuleFor(e => e.Start).NotEmpty();
      RuleFor(e => e.End).NotEmpty().GreaterThan(e => e.Start).WithMessage("'End' must be after 'Start'");

      RuleFor(e => e.Reports).SetCollectionValidator(new ListReportViewModelValidator());
    }
  }


  public class EventViewModelProfile : Profile
  {
    public EventViewModelProfile()
    {
      CreateMap<zulu.Models.Event, EventViewModel>();
    }
  }
}