namespace Weathery.API.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.UserServices;
    using ViewModels;

    // TODO: IMPLEMENT FLUENTVALIDATION!
    // TODO: IMPLEMENT OPERATIONRESULT!
    [Authorize]
    public class UserController : ApiController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService) => this.userService = userService;

        [HttpPost("[action]")]
        public async Task<ActionResult> SaveCity([FromBody] SaveCityViewModel viewModel)
        {
            if (viewModel == null)
            {
                return this.BadRequest($"{nameof(viewModel)} is null.");
            }

            var userId = this.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var operation = await this.userService.SaveCityAsync(userId, viewModel.CityName).ConfigureAwait(false);

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
                    await this.userService.GetAllSavedCities(this.GetLoggedUserId()).ConfigureAwait(false);
                return this.Ok(operation.Data);
            }

            return this.Unauthorized();
        }
    }
}