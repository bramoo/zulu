using AutoMapper;
using FluentValidation;
using System;

namespace zulu.ViewModels.Report
{
  public class CreateReportViewModel
  {
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
  }


  public class CreateReportViewModelValidator : AbstractValidator<CreateReportViewModel>
  {
    public CreateReportViewModelValidator()
    {
      RuleFor(e => e.Title).NotEmpty();
      RuleFor(e => e.Content).NotEmpty();
      RuleFor(e => e.Author).NotEmpty();
    }
  }


  public class CreateReportViewModelProfile : Profile
  {
    public CreateReportViewModelProfile()
    {
      CreateMap<CreateReportViewModel, zulu.Models.Report>();
    }
  }
}