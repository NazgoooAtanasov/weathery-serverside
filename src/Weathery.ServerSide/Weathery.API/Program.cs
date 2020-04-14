namespace Weathery.API
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Defines the starting point of the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The starting point of the application.
        /// </summary>
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        /// <summary>
        /// Creates a default <see cref="IWebHostBuilder"/> for the entire application.
        /// </summary>
        /// <returns>The created <see cref="IWebHostBuilder"/> with pre-configured default and startup type.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}