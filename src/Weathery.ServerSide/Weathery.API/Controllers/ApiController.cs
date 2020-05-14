namespace Weathery.API.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        protected string GetLoggedUserId() => this.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

        protected bool IsAuthenticated() => this.HttpContext.User.Identity.IsAuthenticated;
    }
}