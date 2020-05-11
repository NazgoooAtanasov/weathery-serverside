namespace Weathery.Services.TokenService
{
    using System.Collections.Generic;

    public interface ITokenService
    {
        string CreateToken(string id, string username, string secret);
    }
}
