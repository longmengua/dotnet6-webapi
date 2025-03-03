using dotnet6_webapi.Contexts;
using dotnet6_webapi.Models;
using dotnet6_webapi.Utils;

namespace dotnet6_webapi.Repository;

public class UserRepo
{
    private readonly AppDbContext _context;

    public UserRepo(AppDbContext context)
    {
        _context = context;
    }

    public User Register(string account, string password)
    {
        var user = new User
        {
            Account = account,
            Password = password // You should hash the password before storing it
        };
        _context.Users?.Add(user);
        _context.SaveChanges();
        return user;
    }

    public User? UpdateUser(string account, string password)
    {
        var user = _context.Users?.FirstOrDefault(a => a.Account == account);
        if (user != null)
        {
            user.Password = password; // You should hash the password before storing it
            _context.SaveChanges();
        }
        return user;
    }

    public User? InvokeUser(string account, string password)
    {
        var user = _context.Users?.FirstOrDefault(a => a.Account == account && a.Password == password);
        return user;
    }

    public User? Login(string account, string password)
    {
        var user = _context.Users?.FirstOrDefault(a => a.Account == account && a.Password == password);
        return user;
    }

    public User? RenewRefreshToken(string account)
    {
        var user = _context.Users?.FirstOrDefault(a => a.Account == account);
        if (user != null)
        {
            // Generate a new refresh token
            var refreshToken = AuthHelper.GenerateToken(user, 1);

            // Check if the user has existing refresh tokens
            if (user.UserRefreshTokens?.Any() == true)
            {
                // Mark existing refresh tokens as revoked
                foreach (var r in user.UserRefreshTokens)
                {
                    if (r.RefreshToken != null)
                    {
                        r.RefreshToken.IsRevoked = true;
                    }
                }
            }

            // Add a new refresh token with IsRevoked = false
            user.UserRefreshTokens?.Add(new UserRefreshToken
            {
                RefreshToken = new RefreshToken { Token = refreshToken, IsRevoked = false },
                UserId = user.Id,
            });

            // Save changes to the database
            _context.SaveChanges();
        }

        return user;
    }
}