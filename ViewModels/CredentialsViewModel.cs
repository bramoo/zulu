using FluentValidation;

namespace zulu.ViewModels
{
  public class CredentialsViewModel
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }

  public class CredentialsViewModelValidator : AbstractValidator<CredentialsViewModel>
  {
    public CredentialsViewModelValidator()
    {
      RuleFor(c => c.Email).NotNull().EmailAddress();
      RuleFor(c => c.Password).NotNull().MinimumLength(6);
    }
  }
}
