using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using zulu.ViewModels.Report;

namespace zulu.ViewModels.Event
{
  public class CreateEventViewModel
  {
    public string Name { get; set; }
    public bool AllDay { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }

    public IEnumerable<CreateReportViewModel> Reports { get; set; }
  }


  public class CreateEventViewModelValidator : AbstractValidator<CreateEventViewModel>
  {
    public CreateEventViewModelValidator()
    {
      RuleFor(e => e.Name).NotEmpty();
      RuleFor(e => e.Start).NotEmpty();
      RuleFor(e => e.End).NotEmpty().GreaterThan(e => e.Start).WithMessage("'End' must be greater than 'Start'");

      RuleFor(e => e.Reports).SetCollectionValidator(new CreateReportViewModelValidator());
    }
  }


  public class CreateEventViewModelProfile : Profile
  {
    public CreateEventViewModelProfile()
    {
      CreateMap<zulu.Models.EventReport, CreateReportViewModel>()
          .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Report.Title))
          .ForMember(d => d.Content, opt => opt.MapFrom(src => src.Report.Content))
          .ForMember(d => d.Author, opt => opt.MapFrom(src => src.Report.Author))
          .ReverseMap();

      CreateMap<CreateEventViewModel, zulu.Models.Event>();
    }
  }
}