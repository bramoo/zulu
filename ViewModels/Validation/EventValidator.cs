using FluentValidation;
using zulu.Models;

public class EventValidator : AbstractValidator<Event>
{
  protected EventValidator()
  {
    RuleFor(e => e.Name).NotEmpty();
    RuleFor(e => e.Start).NotEmpty();
    RuleFor(e => e.End).GreaterThan(e => e.Start);
  }
}
