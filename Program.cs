using Serilog;
using dotnet6_webapi.Utils;
using dotnet6_webapi.Middleware;
using dotnet6_webapi.Contexts;

var builder = WebApplication.CreateBuilder(args);

// 讀取預設 `appsettings.json`
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory()) // 設定根目錄
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args);

// 添加服務到容器
builder.Services.AddControllers();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>() // 從當前專案的 `Program` 類別所在的 Assembly 掃描
    .AddClasses(classes => classes.InNamespaces("dotnet6_webapi.Service", "dotnet6_webapi.Repository")) // 針對指定命名空間掃描
    .AsSelf() // 讓 Scrutor 直接註冊類別本身
    .AsImplementedInterfaces() // 也同時以介面形式註冊，適用於依賴介面的類別
    .WithScopedLifetime()); // 設定 `Scoped` 生命週期

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

// 配置 DB 設定
builder.Services.AddDbContext<AppDbContext>(options =>
{
    DBHelper.Init(options, builder.Configuration);
});

// 配置 Serilog 記錄到 ELK
builder.Host.UseSerilog((context, configuration) =>
{
    LoggingHelper.Init(context, configuration, builder.Configuration); // 調用封裝方法來設定日誌
});

// 配置 JWT token 機制
// var jwtSettings = builder.Configuration.GetSection("Jwt");
// AuthHelper.SetConfiguration(
//   jwtSettings.GetValue<string>("SecretKey"),
//   jwtSettings.GetValue<string>("Issuer"),
//   jwtSettings.GetValue<string>("Audience")
// );
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
// {
//     AuthHelper.Init(options);
// });

// 開發環境配置
if (builder.Environment.IsDevelopment())
{
    // 更多關於配置 Swagger/OpenAPI 的資訊，請參見 https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        SwaggerHelper.Init(c); // 啟用 JWT token 功能
    });
}




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
    // 在此可加入關閉資源的邏輯，例如關閉資料庫連線、停止背景任務等
    logger.LogInformation("應用程式正在關閉...");
    // 模擬等待5秒，給目前的請求更多時間完成
    // 也可以用 docker run --stop-timeout=5s <your_image> 達到同樣效果
    System.Threading.Thread.Sleep(5000);
    logger.LogInformation("已等待5秒，應用程式關閉完成。");
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

// 使用自定義的請求超時中間件，10秒API就timeout
app.UseMiddleware<TimeoutMiddleware>(TimeSpan.FromSeconds(10));

// 啟用 HTTPS 重定向
// app.UseHttpsRedirection();

// 啟用授權 (.net 內建)
// app.UseAuthentication(); // 先認證用戶身份
// app.UseAuthorization(); // 然後根據身份授權

// 客製化的 Auth
app.UseMiddleware<AuthMiddleware>();

// 啟用 CORS
app.UseCors("AllowAll");

// 註冊控制器路由
app.MapControllers();

// 啟動應用程式
app.Run();
