namespace Weathery.API.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.UserService;
    using ViewModels;

    // TODO: IMPLEMENT FLUENTVALIDATION!
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) => this._userService = userService;

        [HttpPost("[action]")]
        public async Task<ActionResult> SaveCity([FromBody] SaveCityViewModel viewModel)
        {
            if (viewModel == null)
            {
                return this.BadRequest($"{nameof(viewModel)} is null.");
            }

            var userId = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var operation = await this._userService.SaveCityAsync(userId, viewModel.CityName).ConfigureAwait(false);

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