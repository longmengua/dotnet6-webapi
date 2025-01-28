using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace dotnet6_webapi.Utils;

public static class LoggingHelper
{
    // 配置 Serilog 記錄器
    public static void ConfigureLogging(HostBuilderContext context, LoggerConfiguration configuration)
    {
        configuration
            .Enrich.FromLogContext() // 從日志上下文中豐富日誌
            .WriteTo.Console() // 控制台輸出
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
            {
                IndexFormat = "dotnet-webapi-logs-{0:yyyy.MM.dd}", // 設定索引名稱格式
                AutoRegisterTemplate = true, // 自動註冊 Elasticsearch 的索引模板
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8, // 使用 Elasticsearch 版本 8 的模板
                NumberOfShards = 1, // 設定分片數量
                NumberOfReplicas = 1 // 設定副本數量
            })
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName) // 添加環境變數
        .Enrich.WithProperty("Application", "DotnetWebApi"); // 添加應用程式名稱
    }
}