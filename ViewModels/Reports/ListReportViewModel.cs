using AutoMapper;
using FluentValidation;
using System;

namespace zulu.ViewModels.Report
{
  public class ListReportViewModel
  {
    public int Id { get; set; }
    public string State { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set; }

    public string Title { get; set; }
    public string Author { get; set; }
  }


  public class ListReportViewModelValidator : AbstractValidator<ListReportViewModel>
  {
    public ListReportViewModelValidator()
    {
      RuleFor(e => e.Id).NotEmpty();
      RuleFor(e => e.Title).NotEmpty();
      RuleFor(e => e.Author).NotEmpty();
    }
  }


  public class ListReportViewModelProfile : Profile
  {
    public ListReportViewModelProfile()
    {
      CreateMap<zulu.Models.Report, ListReportViewModel>();
    }
  }
}