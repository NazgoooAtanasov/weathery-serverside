namespace Weathery.API
{
    using System.Text;
    using Extensions;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Utilities.AuthenticationUtilities;

    /// <summary>Defines the startup class for the application.</summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Startup" /> class.
        /// </summary>
        /// <param name="configuration">
        ///     The <see cref="IConfiguration" /> for the application.
        /// </param>
        public Startup(IConfiguration configuration) => this._configuration = configuration;

        /// <summary>
        ///     Method that adds all the needed services for the application.
        /// </summary>
        /// <param name="services">
        ///     A collection of all services needed.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = this._configuration.GetSection(nameof(AuthenticationSettings));
            services.Configure<AuthenticationSettings>(appSettingsSection);

            // Configuring the jwt authentication.
            var appSettings = appSettingsSection.Get<AuthenticationSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            JwtTokenAuthentication.JwtTokenSetUp(services, key);

            // Configuring the database.
            DatabaseConfiguration.Setup(this._configuration, services);

            // Configuring the services.
            ServicesConfiguration.Setup(services);
        }

        /// <summary>
        ///     Method that gets called by the runtime.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder" /> for the application.</param>
        /// <param name="env">The <see cref="IWebHostEnvironment" /> for the application.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(options => options
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}