using FluentValidation;
using zulu.ViewModels;

public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
{
  protected RegistrationViewModelValidator()
  {
    RuleFor(r => r.Email).NotNull().EmailAddress();
    RuleFor(r => r.Password).NotNull().MinimumLength(6);
  }
}