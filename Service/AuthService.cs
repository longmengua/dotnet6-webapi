using dotnet6_webapi.DTO.Res;
using dotnet6_webapi.Repositroy;
using Serilog;

namespace dotnet6_webapi.Service;
public class AuthService
{
    private readonly UserRepo authRepo;

    public AuthService(UserRepo authRepo)
    {
        this.authRepo = authRepo;
    }

    public Auth AuthenticateUser(string account, string password)
    {
        var auth = authRepo.Login(account, password);
        var toReturn = new Auth { Account = auth.Account };
        return toReturn;
    }
}