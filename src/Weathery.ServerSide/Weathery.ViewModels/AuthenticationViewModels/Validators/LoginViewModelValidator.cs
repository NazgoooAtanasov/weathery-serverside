namespace Weathery.ViewModels.Authentication.Validators
{
    using FluentValidation;

    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            this.RuleFor(x => x.Username).NotNull().NotEmpty().MinimumLength(5).MaximumLength(15);
            this.RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(10);
        }
    }
}