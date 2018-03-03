using FluentValidation;

namespace zulu.ViewModels
{
  public class RegistrationViewModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class RegistrationViewModelValidator : AbstractValidator<RegistrationViewModel>
  {
    public RegistrationViewModelValidator()
    {
      RuleFor(r => r.Email).NotNull().EmailAddress();
      RuleFor(r => r.Password).NotNull().MinimumLength(6);
    }
  }
}
