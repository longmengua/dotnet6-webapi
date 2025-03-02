using dotnet6_webapi.DTO.Res;

namespace dotnet6_webapi.Repositroy;
public class AuthRepo
{
    public Auth VerifyUser(string account, string password)
    {
        var auth = new Auth() { };
        return auth;
    }
}