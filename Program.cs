using Serilog;
using dotnet6_webapi.Utils;
using dotnet6_webapi.Middlewares;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 添加服務到容器
builder.Services.AddControllers();
// 更多關於配置 Swagger/OpenAPI 的資訊，請參見 https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 設定 CORS，允許所有來源的請求
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin() // 允許來自任何來源的請求
              .AllowAnyMethod() // 允許任何 HTTP 方法
              .AllowAnyHeader(); // 允許任何標頭
    });
});

// 配置 Serilog 記錄到 ELK
builder.Host.UseSerilog((context, configuration) =>
{
    LoggingHelper.ConfigureLogging(context, configuration); // 調用封裝方法來設定日誌
});

var app = builder.Build();

// 配置優雅關閉
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

lifetime.ApplicationStarted.Register(() =>
{
    logger.LogInformation("應用程式已啟動。");
});

lifetime.ApplicationStopping.Register(() =>
{
    logger.LogInformation("應用程式正在關閉...");
    // 在此可加入關閉資源的邏輯，例如關閉資料庫連線、停止背景任務等
});

lifetime.ApplicationStopped.Register(() =>
{
    logger.LogInformation("應用程式已停止。");
});

// 開發環境配置
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // 啟用 Swagger
    app.UseSwaggerUI(); // 啟用 Swagger UI
}

// 使用自定義的異常處理中間件
app.UseMiddleware<ExceptionHandlingMiddleware>();

// 啟用 HTTPS 重定向
app.UseHttpsRedirection();

// 啟用授權
app.UseAuthorization();

// 啟用 CORS
app.UseCors("AllowAll");

// 註冊控制器路由
app.MapControllers();

// 啟動應用程式
app.Run();
