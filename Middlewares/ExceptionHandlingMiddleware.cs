using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace dotnet6_webapi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
            _logger.LogError(ex, "發生了未處理的異常");

            // 回傳自定義的錯誤訊息給客戶端
            httpContext.Response.StatusCode = 500; // 內部伺服器錯誤
            httpContext.Response.ContentType = "application/json";
            var result = new { message = "系統發生錯誤，請稍後再試。" };

            await httpContext.Response.WriteAsJsonAsync(result);
        }
    }
}

