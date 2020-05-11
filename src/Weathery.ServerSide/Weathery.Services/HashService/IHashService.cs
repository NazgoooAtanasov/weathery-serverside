namespace Weathery.Services.HashService
{
    public interface IHashService
    {
        string Hash(string rawData, string username);
    }
}
