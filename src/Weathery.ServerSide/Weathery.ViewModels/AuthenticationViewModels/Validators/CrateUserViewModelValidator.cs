namespace Weathery.ViewModels.Authentication.Validators
{
    using FluentValidation;

    public class CrateUserViewModelValidator : AbstractValidator<CreateUserViewModel>
    {
        public CrateUserViewModelValidator()
        {
            this.RuleFor(x => x.Username).NotNull().NotEmpty().MinimumLength(5).MaximumLength(15);
            this.RuleFor(x => x.Password).NotNull().NotEmpty().MinimumLength(10);
        }
    }
}