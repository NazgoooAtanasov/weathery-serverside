namespace Weathery.API
{
    using System.Text;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Weathery.API.Utilities;
    using Weathery.Services.HashService;
    using Weathery.Services.TokenService;
    using Weathery.Services.UserServices;
    using Weathery.Utilities.AuthenticationUtilities;

    /// <summary>Defines the startup class for the application.</summary>
    public class Startup
    {
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The <see cref="IConfiguration"/> for the application.
        /// </param>
        public Startup(IConfiguration configuration) => this.configuration = configuration;

        /// <summary>
        /// Method that adds all the needed services for the application.
        /// </summary>
        /// <param name="services">
        /// A collection of all services needed.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = this.configuration.GetSection(nameof(AuthenticationSettings));
            services.Configure<AuthenticationSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AuthenticationSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            Extensions.JWTTokenAuthentication.JWTTokenSetUp(services, key);

            services.Configure<WeatheryDatabaseSettings>(this.configuration.GetSection(nameof(WeatheryDatabaseSettings)));
            services.AddSingleton<IWeatheryDatabaseSettings>(sp => sp.GetRequiredService<IOptions<WeatheryDatabaseSettings>>().Value);

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IHashService, HashService>();

            services.AddControllers();
        }

        /// <summary>
        /// Method that gets called by the runtime.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> for the application.</param>
        /// <param name="env">The <see cref="IWebHostEnvironment"/> for the application.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}