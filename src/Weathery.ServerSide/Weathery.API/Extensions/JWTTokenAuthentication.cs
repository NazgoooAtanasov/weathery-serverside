namespace Weathery.API.Extensions
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    /// <summary>
    ///     Base class that sets up the configuration for the Jwt authentication.
    /// </summary>
    public static class JwtTokenAuthentication
    {
        /// <summary>
        ///     Sets up the configuration for the Jwt authentication.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection" /> to store the service.</param>
        /// <param name="key">The secret.</param>
        public static void JwtTokenSetUp(IServiceCollection services, byte[] key)
            =>
                services.AddAuthentication(x =>
                    {
                        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(x =>
                    {
                        x.RequireHttpsMetadata = false;
                        x.SaveToken = true;
                        x.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true, IssuerSigningKey = new SymmetricSecurityKey(key)
                            , ValidateIssuer = false, ValidateAudience = false,
                        };
                    });
    }
}