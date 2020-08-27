namespace Weathery.Services.TokenService
{
    public interface ITokenService
    {
        string CreateToken(string id, string username, string secret);
    }
}