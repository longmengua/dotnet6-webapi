using dotnet6_webapi.Models;

namespace dotnet6_webapi.Repositroy;
public class AuthRepo
{
    public Auth VerifyUser(string account, string password)
    {
        var auth = new Auth() { };
        return auth;
    }
}