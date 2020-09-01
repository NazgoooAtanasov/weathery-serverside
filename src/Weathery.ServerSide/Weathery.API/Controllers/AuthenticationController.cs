namespace Weathery.API.Controllers
{
    using System.Threading.Tasks;
    using FluentValidation;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Services.TokenService;
    using Services.UserService;
    using Utilities.AuthenticationUtilities;
    using ViewModels.Authentication;

    public class AuthenticationController : ApiController
    {
        private readonly IOptions<AuthenticationSettings> _settings;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IValidator<CreateUserViewModel> _createUserValidator;
        private readonly IValidator<LoginViewModel> _loginUserValidator;

        public AuthenticationController(
            IUserService userService,
            IOptions<AuthenticationSettings> authSettings,
            ITokenService tokenService,
            IValidator<CreateUserViewModel> createUserValidator,
            IValidator<LoginViewModel> loginUserValidator)
        {
            this._userService = userService;
            this._settings = authSettings;
            this._tokenService = tokenService;
            this._createUserValidator = createUserValidator;
            this._loginUserValidator = loginUserValidator;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Register([FromBody] CreateUserViewModel viewModel)
        {
            var validate = await this._createUserValidator.ValidateAsync(viewModel).ConfigureAwait(false);
            if (!validate.IsValid)
            {
                this.BadRequest(validate.Errors);
            }

            var operation = await this._userService.CreateAsync(viewModel?.Username, viewModel?.Password)
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

            var validate = await this._loginUserValidator.ValidateAsync(viewModel).ConfigureAwait(false);
            if (!validate.IsValid)
            {
                this.BadRequest(validate.Errors);
            }

            var user = await this._userService.GetAsync(viewModel?.Username).ConfigureAwait(false);

            if (user == null)
            {
                return this.BadRequest($"{nameof(user)} with this information was not found.");
            }

            // TODO: Create a separate model for JWT Token response!
            return new TokenViewModel
            {
                Token =
                    this._tokenService.CreateToken(user.Data.Id, user.Data.Username, this._settings.Value.Secret)
                ,
            };
        }
    }
}