using AutoMapper;
using FluentValidation;
using System;

namespace zulu.ViewModels.Report
{
  public class EditReportViewModel
  {
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
  }


  public class EditReportViewModelValidator : AbstractValidator<EditReportViewModel>
  {
    public EditReportViewModelValidator()
    {
      RuleFor(e => e.Title).NotEmpty();
      RuleFor(e => e.Content).NotEmpty();
      RuleFor(e => e.Author).NotEmpty();
    }
  }


  public class EditReportViewModelProfile : Profile
  {
    public EditReportViewModelProfile()
    {
      CreateMap<EditReportViewModel, zulu.Models.Report>()
          .ForMember(e => e.Id, opt => opt.Ignore()); ;
    }
  }
}