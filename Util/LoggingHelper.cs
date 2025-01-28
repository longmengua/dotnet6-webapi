using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace dotnet6_webapi.Util // 替换为你的命名空间
{
    public static class LoggingHelper
    {
        public static void ConfigureLogging(HostBuilderContext context, LoggerConfiguration configuration)
        {
            configuration
                .Enrich.FromLogContext()
                .WriteTo.Console() // 控制台输出
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    IndexFormat = "dotnet-webapi-logs-{0:yyyy.MM.dd}", // 索引名称
                    AutoRegisterTemplate = true, // 自动注册 Elasticsearch 的索引模板
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                    NumberOfShards = 1,
                    NumberOfReplicas = 1
                })
                .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName) // 添加环境变量
                .Enrich.WithProperty("Application", "DotnetWebApi") // 添加应用名称
                .ReadFrom.Configuration(context.Configuration); // 从 appsettings.json 读取额外配置
        }
    }
}
