namespace Weathery.Utilities.DatabaseUtilities
{
    using API.Utilities;

    public class WeatheryDatabaseSettings : IWeatheryDatabaseSettings
    {
        public string UsersCollection { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}