namespace Weathery.API.Utilities
{
    public interface IWeatheryDatabaseSettings
    {
        string UsersCollection { get; set; }

        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}