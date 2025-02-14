using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace dotnet6_webapi.Utils;

/// <summary>
/// 加上那個「鎖」的 icon 到 swagger，讓 swagger 可以直接帶 JWT token 做 API 測試
/// </summary>
public class SwaggerHelper
{
    public static void Init(SwaggerGenOptions c)
    {
        // Add JWT Bearer Authorization
        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "Enter your JWT token in the format 'Bearer {your token}'"
        });

        // Add JWT Bearer to global security requirements
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

        // Optionally, add additional Swagger configuration, like versioning or description
        // c.SwaggerDoc("v1", new OpenApiInfo { Title = "API V1", Version = "v1" });
    }
}