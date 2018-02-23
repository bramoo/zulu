using FluentValidation;
using zulu.ViewModels;

public class CredentialsViewModelValidator : AbstractValidator<CredentialsViewModel>
{
  protected CredentialsViewModelValidator()
  {
    RuleFor(c => c.Email).NotNull().EmailAddress();
    RuleFor(c => c.Password).NotNull().MinimumLength(6);
  }
}