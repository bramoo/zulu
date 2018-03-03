using AutoMapper;
using FluentValidation;
using System;

namespace zulu.ViewModels.Report
{
  public class ReportViewModel
  {
    public int Id { get; set; }
    public string State { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }

    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
  }


  public class ReportViewModelValidator : AbstractValidator<ReportViewModel>
  {
    public ReportViewModelValidator()
    {
      RuleFor(e => e.Id).NotEmpty();
      RuleFor(e => e.Title).NotEmpty();
      RuleFor(e => e.Content).NotEmpty();
      RuleFor(e => e.Author).NotEmpty();
    }
  }


  public class ReportViewModelProfile : Profile
  {
    public ReportViewModelProfile()
    {
      CreateMap<zulu.Models.Report, ReportViewModel>();
    }
  }
}