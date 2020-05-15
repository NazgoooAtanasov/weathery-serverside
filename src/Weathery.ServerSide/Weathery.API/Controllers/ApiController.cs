namespace Weathery.API.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>Base controller for the application.</summary>
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        /// <summary>Gets the currently logged user's id.</summary>
        /// <returns>The users id.</returns>
        protected string GetLoggedUserId() => this.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

        /// <summary>Checks wether there is logged user.</summary>
        /// <returns>True of false.</returns>
        protected bool IsAuthenticated() => this.HttpContext.User.Identity.IsAuthenticated;
    }
}