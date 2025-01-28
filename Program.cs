using Serilog;
using dotnet6_webapi.Utils;

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

// 配置 Serilog 設定 ELK
builder.Host.UseSerilog((context, configuration) =>
{
    LoggingHelper.ConfigureLogging(context, configuration); // 調用封裝方法來設定日誌
});

var app = builder.Build();

// 開發環境配置
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // 啟用 Swagger
    app.UseSwaggerUI(); // 啟用 Swagger UI
}

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
