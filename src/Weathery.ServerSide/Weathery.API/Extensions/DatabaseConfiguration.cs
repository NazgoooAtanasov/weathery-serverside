namespace Weathery.API.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Utilities.DatabaseUtilities;

    /// <summary>Class used to configure the database.</summary>
    public static class DatabaseConfiguration
    {
        /// <summary>
        ///     Used to configure everything needed for the database.
        /// </summary>
        /// <param name="configuration">The current <see cref="IConfiguration" /> of the application.</param>
        /// <param name="services">The current collection of <see cref="IServiceCollection" /> of the application.</param>
        public static void Setup(IConfiguration configuration, IServiceCollection services)
        {
            services.Configure<WeatheryDatabaseSettings>(
                configuration?.GetSection(nameof(WeatheryDatabaseSettings)));
            services.AddSingleton<IWeatheryDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<WeatheryDatabaseSettings>>().Value);
        }
    }
}