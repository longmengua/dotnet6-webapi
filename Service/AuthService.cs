using dotnet6_webapi.DTO.Res;
using dotnet6_webapi.Repository;
using dotnet6_webapi.Utils;
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
        var toReturn = new Auth
        {
            Account = user?.Account,
            FirstName = user?.FirstName,
            MiddleName = user?.MiddleName,
            LastName = user?.LastName,
            Email = user?.Email,
            Phone = user?.Phone,
            CreatedAt = user?.CreatedAt ?? DateTimeOffset.UtcNow,
            UpdatedAt = user?.UpdatedAt ?? DateTimeOffset.UtcNow
        };
        return toReturn;
    }

    public (Auth, string, string) RegisterUser(string account, string password, string firstName, string? middleName, string? lastName, string? email, string? phone)
    {
        var user = userRepo.Register(account, password, firstName, middleName, lastName, email, phone);
        var auth = new Auth
        {
            Account = user.Account,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            LastName = user.LastName,
            Email = user.Email,
            Phone = user.Phone,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };

        // Generate JWT token
        var jwtToken = AuthHelper.GenerateToken(new { Account = user.Account });

        // Generate Refresh token
        var refreshToken = GenerateRefreshToken();
        userRepo.SaveRefreshToken(user.Id, refreshToken);

        return (auth, jwtToken, refreshToken);
    }

    public Auth UpdateUserPassword(string account, string newPassword)
    {
        var user = userRepo.UpdateUser(account, newPassword);
        var toReturn = new Auth
        {
            Account = user?.Account,
            FirstName = user?.FirstName,
            MiddleName = user?.MiddleName,
            LastName = user?.LastName,
            Email = user?.Email,
            Phone = user?.Phone,
            CreatedAt = user?.CreatedAt ?? DateTimeOffset.UtcNow,
            UpdatedAt = user?.UpdatedAt ?? DateTimeOffset.UtcNow
        };
        return toReturn;
    }

    public Auth RenewUserRefreshToken(string account)
    {
        var user = userRepo.RenewRefreshToken(account);
        var toReturn = new Auth
        {
            Account = user?.Account,
            FirstName = user?.FirstName,
            MiddleName = user?.MiddleName,
            LastName = user?.LastName,
            Email = user?.Email,
            Phone = user?.Phone,
            CreatedAt = user?.CreatedAt ?? DateTimeOffset.UtcNow,
            UpdatedAt = user?.UpdatedAt ?? DateTimeOffset.UtcNow
        };
        return toReturn;
    }

    private string GenerateRefreshToken()
    {
        // Implement your refresh token generation logic here
        return Guid.NewGuid().ToString("N");
    }
}