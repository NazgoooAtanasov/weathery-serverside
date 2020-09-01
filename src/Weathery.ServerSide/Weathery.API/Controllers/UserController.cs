namespace Weathery.API.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using FluentValidation;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.UserService;
    using ViewModels;

    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        private readonly IValidator<SaveCityViewModel> _saveCityValidator;

        public UserController(IUserService userService, IValidator<SaveCityViewModel> saveCityValidator)
        {
            this._userService = userService;
            this._saveCityValidator = saveCityValidator;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> SaveCity([FromBody] SaveCityViewModel viewModel)
        {
            var validate = await this._saveCityValidator.ValidateAsync(viewModel).ConfigureAwait(false);
            if (!validate.IsValid)
            {
                return this.BadRequest(validate.Errors);
            }

            var userId = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var operation = await this._userService.SaveCityAsync(userId, viewModel?.CityName).ConfigureAwait(false);

            if (operation.Success == false)
            {
                return this.BadRequest(operation.ErrorMessages);
            }

            return this.Ok();
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> SavedCities()
        {
            if (this.IsAuthenticated())
            {
                var operation =
                    await this._userService.GetAllSavedCities(this.GetLoggedUserId()).ConfigureAwait(false);
                return this.Ok(operation.Data);
            }

            return this.Unauthorized();
        }
    }
}