using dotnet6_webapi.DTO.Res;
using dotnet6_webapi.Repository;
using Serilog;

namespace dotnet6_webapi.Service;
public class AuthService
{
    private readonly UserRepo userRepo;

    public AuthService(UserRepo userRepo)
    {
        this.userRepo = userRepo;
    }

    public Auth AuthenticateUser(string account, string password)
    {
        var user = userRepo.Login(account, password);
        var toReturn = new Auth { Account = user?.Account };
        return toReturn;
    }

    public Auth RegisterUser(string account, string password)
    {
        var user = userRepo.Register(account, password);
        var toReturn = new Auth { Account = user.Account };
        return toReturn;
    }

    public Auth UpdateUserPassword(string account, string newPassword)
    {
        var user = userRepo.UpdateUser(account, newPassword);
        var toReturn = new Auth { Account = user?.Account };
        return toReturn;
    }

    public Auth RenewUserRefreshToken(string account)
    {
        var user = userRepo.RenewRefreshToken(account);
        var toReturn = new Auth { Account = user?.Account };
        return toReturn;
    }
}