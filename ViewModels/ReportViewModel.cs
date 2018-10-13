using FluentValidation;
using System;

namespace zulu.ViewModels
{
  public class ReportViewModel : EntityViewModel
  {
    public string Title { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
  }


  public class ReportViewModelValidator : AbstractValidator<ReportViewModel>
  {
    public ReportViewModelValidator()
    {
      RuleFor(e => e.Title).NotEmpty();
      RuleFor(e => e.Content).NotEmpty();
      RuleFor(e => e.Author).NotEmpty();
    }
  }
}