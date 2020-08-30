namespace Weathery.Utilities.DatabaseUtilities
{
    public interface IWeatheryDatabaseSettings
    {
        string UsersCollection { get; set; }

        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}