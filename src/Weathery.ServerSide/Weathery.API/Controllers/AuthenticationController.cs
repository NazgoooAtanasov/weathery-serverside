namespace Weathery.API.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Weathery.Services.TokenService;
    using Weathery.Services.UserServices;
    using Weathery.Utilities.AuthenticationUtilities;
    using Weathery.ViewModels;
    using Weathery.ViewModels.Authentication;

    public class AuthenticationController : ApiController
    {
        private readonly IUserService userService;
        private readonly IOptions<AuthenticationSettings> settings;
        private readonly ITokenService tokenService;

        public AuthenticationController(
            IUserService userService,
            IOptions<AuthenticationSettings> authSettings,
            ITokenService tokenService)
        {
            this.userService = userService;
            this.settings = authSettings;
            this.tokenService = tokenService;
        }


        [HttpPost("[action]")]
        public async Task<ActionResult> Register([FromBody]CreateUserViewModel viewModel)
        {
            if (viewModel == null)
            {
                return this.BadRequest($"{nameof(viewModel)} should not be empty");
            }

            var operation = await this.userService.CreateAsync(viewModel.Username, viewModel.Password).ConfigureAwait(false);
            if (operation == true)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest("Username already taken.");
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<object>> Login([FromBody]LoginViewModel viewModel)
        {
            if (this.IsAuthenticated())
            {
                return this.BadRequest("Already logged in.");
            }

            if (viewModel == null)
            {
                return this.BadRequest($"{nameof(viewModel)} should not be empty");
            }

            var user = await this.userService.GetAsync(viewModel.Username).ConfigureAwait(false);

            if (user == null)
            {
                return this.BadRequest($"{nameof(user)} with this inforamtion was not found.");
            }

            return new TokenViewModel
            {
                Token =
                    this.tokenService.CreateToken(user.Id, user.Username, this.settings.Value.Secret),
            };
        }
    }
}