using System.Security.Claims;
using dotnet6_webapi.Utils;
using Serilog;

namespace dotnet6_webapi.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly HashSet<string> _whitelistPaths = new()
    {
        "/api/publics",
    };

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLower();
        Log.Information($"Request path: {path}");
        // 若在白名單中，直接放行
        if (path != null)
        {
            if ("/".Equals(path) || _whitelistPaths.Any(p => path.StartsWith(p)))
            {
                await _next(context);
                return;
            }
        }

        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (string.IsNullOrEmpty(token))
        {
            Log.Warning("Authorization token is missing.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Token is required");
            return;
        }

        if (!AuthHelper.VerifyToken(token, out ClaimsPrincipal? principal) || principal == null)
        {
            Log.Warning("Invalid or expired JWT token.");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid token");
            return;
        }

        context.User = principal;
        await _next(context);
    }
}
