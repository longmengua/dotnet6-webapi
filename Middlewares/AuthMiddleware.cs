using System.Security.Claims;
using dotnet6_webapi.Utils;
using Serilog;

namespace dotnet6_webapi.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    // 建構函式，初始化超時時間
    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
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
