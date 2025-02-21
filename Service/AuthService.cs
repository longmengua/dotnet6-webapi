using dotnet6_webapi.DTOs;
using dotnet6_webapi.Repositroy;
using Serilog;

namespace dotnet6_webapi.Service;
public class AuthService
{
    private readonly AuthRepo authRepo;

    public AuthService(AuthRepo authRepo)
    {
        this.authRepo = authRepo;
    }

    public AuthRes AuthenticateUser(string account, string password)
    {
        var auth = authRepo.VerifyUser(account, password);
        var toReturn = new AuthRes { Account = auth.Account };
        return toReturn;
    }
}