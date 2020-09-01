namespace Weathery.ViewModels.Validators
{
    using FluentValidation;

    public class SaveCityViewModelValidator : AbstractValidator<SaveCityViewModel>

    {
        public SaveCityViewModelValidator()
        {
            this.RuleFor(x => x.CityName).NotNull().NotEmpty();
        }
    }
}