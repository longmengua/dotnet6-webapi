using Serilog;

namespace dotnet6_webapi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext); // 呼叫下一個中間件
        }
        catch (Exception ex)
        {
            // 捕獲例外並記錄
            Log.Information(ex, "發生了未處理的異常");

            // 回傳自定義的錯誤訊息給客戶端
            httpContext.Response.StatusCode = 500; // 內部伺服器錯誤
            httpContext.Response.ContentType = "application/json";
            var result = new { message = "系統發生錯誤，請稍後再試。" };

            await httpContext.Response.WriteAsJsonAsync(result);
        }
    }
}

