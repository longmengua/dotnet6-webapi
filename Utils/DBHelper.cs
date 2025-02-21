using Microsoft.EntityFrameworkCore;

namespace dotnet6_webapi.Utils;
public class DBHelper
{
    public static void Init(DbContextOptionsBuilder options, ConfigurationManager configuration)
    {
        var connectionStr = configuration.GetConnectionString("DefaultConnection");

        if (connectionStr == null)
        {
            throw new ArgumentNullException(nameof(connectionStr), "Connection string 'DefaultConnection' is missing in the configuration.");
        }
        // 讀取資料庫設定 (MSSQL)
        // options.UseSqlServer(connectionStr);
        // 讀取資料庫設定 (PostgreqSQL)
        options.UseNpgsql(connectionStr);
    }
}