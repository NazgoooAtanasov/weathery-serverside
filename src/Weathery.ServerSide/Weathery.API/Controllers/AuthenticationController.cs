namespace Weathery.API.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Services.TokenService;
    using Services.UserServices;
    using ViewModels;
    using ViewModels.Authentication;
    using Weathery.Utilities.AuthenticationUtilities;

    // TODO: IMPLEMENT FLUENTVALIDATION!
    // TODO: IMPLEMENT OPERATIONRESULT!
    public class AuthenticationController : ApiController
    {
        private readonly IOptions<AuthenticationSettings> _settings;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;

        public AuthenticationController(
            IUserService userService,
            IOptions<AuthenticationSettings> authSettings,
            ITokenService tokenService)
        {
            this._userService = userService;
            this._settings = authSettings;
            this._tokenService = tokenService;
        }


        [HttpGet("[action]")]
        public async Task<ActionResult> Hey()
        {
            return this.Ok("heyt");
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register([FromBody] CreateUserViewModel viewModel)
        {
            if (viewModel == null)
            {
                return this.BadRequest($"{nameof(viewModel)} should not be empty");
            }

            var operation = await this._userService.CreateAsync(viewModel.Username, viewModel.Password)
                .ConfigureAwait(false);
            if (operation.Success == false)
            {
                return this.BadRequest(operation.ErrorMessages);
            }

            return this.Ok();
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<object>> Login([FromBody] LoginViewModel viewModel)
        {
            if (this.IsAuthenticated())
            {
                return this.BadRequest("Already logged in.");
            }

            if (viewModel == null)
            {
                return this.BadRequest($"{nameof(viewModel)} should not be empty");
            }

            var user = await this._userService.GetAsync(viewModel.Username).ConfigureAwait(false);

            if (user == null)
            {
                return this.BadRequest($"{nameof(user)} with this information was not found.");
            }

            // TODO: Create a separate model for JWT Token response!
            return new TokenViewModel
            {
                Token =
                    this._tokenService.CreateToken(user.Data.Id, user.Data.Username, this._settings.Value.Secret),
            };
        }
    }
}