using Microsoft.AspNetCore.Mvc;
using MiniSkeletonAPI.Application.Common.Interfaces;
using MiniSkeletonAPI.Infrastructure.Data;
using MiniSkeletonAPI.Presentation.Services;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace MiniSkeletonAPI.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();
        services.AddExceptionHandler<CustomExceptionHandler>();
        //services.AddProblemDetails();
        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();

        services.AddOpenApiDocument((configure, sp) =>
        {
            configure.Title = "CleanArchitecture API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        return services;
    }
}


//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using MiniSkeletonAPI.Application.Common.Interfaces;
//using MiniSkeletonAPI.Infrastructure.Data;
//using MiniSkeletonAPI.Presentation.Services;
//using MiniSkeletonAPI.Presentation.Settings;
//using NSwag.Generation.Processors.Security;
//using NSwag;
//using System.Text;

//public static class DependencyInjection
//{
//    public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
//    {

//        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();


//        services.AddAuthentication(options =>
//        {
//            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//        })
//        .AddJwtBearer(options =>
//        {
//            options.TokenValidationParameters = new TokenValidationParameters
//            {
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidateLifetime = true,
//                ValidateIssuerSigningKey = true,
//                ValidIssuer = jwtSettings.Issuer,
//                ValidAudience = jwtSettings.Audience,
//                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
//            };
//        });

//        // Layanan lainnya seperti kesehatan dan lainnya
//        services.AddDatabaseDeveloperPageExceptionFilter();
//        services.AddScoped<IUser, CurrentUser>();
//        services.AddHttpContextAccessor();
//        services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
//        services.AddEndpointsApiExplorer();

//        // Menambahkan Swagger dan autentikasi JWT
//        services.AddSwaggerWithJwt();

//        return services;
//    }

//    // Fungsi AddSwaggerWithJwt masih bisa tetap digunakan jika diperlukan untuk konfigurasi Swagger
//    private static IServiceCollection AddSwaggerWithJwt(this IServiceCollection services)
//    {
//        services.AddOpenApiDocument(configure =>
//        {
//            configure.Title = "MiniSkeletonAPI";

//            // Menambahkan pengaturan keamanan untuk Swagger
//            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
//            {
//                Type = OpenApiSecuritySchemeType.ApiKey,
//                Name = "Authorization",
//                In = OpenApiSecurityApiKeyLocation.Header,
//                Description = "Type into the textbox: Bearer {your JWT token}."
//            });

//            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
//        });

//        return services;
//    }
//}
