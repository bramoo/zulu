using FluentValidation;
using zulu.Models;

public class ReportValidator : AbstractValidator<Report>
{
  protected ReportValidator()
  {
    RuleFor(r => r.Title).NotEmpty();
    RuleFor(r => r.Author).NotEmpty();
    RuleFor(r => r.Content).NotNull().MinimumLength(20);
  }
}