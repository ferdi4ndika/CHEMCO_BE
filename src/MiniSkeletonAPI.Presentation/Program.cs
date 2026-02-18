

using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using MiniSkeletonAPI.Application;
using MiniSkeletonAPI.Infrastructure;
using MiniSkeletonAPI.Infrastructure.Data;
using MiniSkeletonAPI.Presentation;
//using MiniSkeletonAPI.Presentation.Endpoints;
using MiniSkeletonAPI.Presentation.Hubs;using System.Text;
using MiniSkeletonAPI.Presentation.Extentions;
using MiniSkeletonAPI.Presentation.Controllers;
using MiniSkeletonAPI.Infrastructure.Identity;
using MiniSkeletonAPI.Presentation.Services;
using Microsoft.AspNetCore.Diagnostics;
using MiniSkeletonAPI.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 104857600; 
});
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 104857600;
});
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.ConfigureJWT(builder.Configuration);
builder.Services.ConfigureCorsPolicy(builder.Configuration);
builder.Services.AddWebServices();
builder.Services.AddHostedService<MqttBackgroundService>();

//builder.Services.AddWebServices(builder.Configuration);

builder.Services.AddAntiforgery();
builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddHostedService<DataAndonRealtimeService>();
builder.Services.AddHostedService<DataAndonDetailRealtimeService>();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//    });


builder.Services.AddSignalR();
var app = builder.Build();
app.UseHttpsRedirection();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        await ApplicationDbSeeder.SeedAsync(services);
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($"Seeding gagal: {ex.Message}");
//    }
//}




//if (app.Environment.IsDevelopment())
//    await app.InitialiseDatabaseAsync();
//else
//    app.UseHsts();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .HasApiVersion(new ApiVersion(2))
    .ReportApiVersions()
    .Build();

app.UseRouting();                   
app.UseCors("CorsPolicy");
app.UseAuthentication();            
app.UseAuthorization();             
app.UseStaticFiles();                
app.UseHealthChecks("/health");            
app.UseOpenApi();                    
app.UseSwaggerUi();                 
app.UseAntiforgery();               
app.UseReDoc(options =>             
{
    options.Path = "/redoc";
});

//app.UseExceptionHandler(options => { });
app.UseExceptionHandler("/error");
app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception == null) return;

        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        context.Response.ContentType = "application/json";

        context.Response.StatusCode = exception switch
        {
            OperationCanceledException => StatusCodes.Status503ServiceUnavailable,
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        // Log the exception
        logger.LogError($"Error: {exception.Message}");

        // Create and send the error response
        var errorResponse = new
        {
            statusCode = context.Response.StatusCode,
            message = exception.Message
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    });
});




app.MapHub<RealTimeHub>("/hub-andon-data");
app.MapHub<RealTimeHubDetail>("/hub-andon-data-detail");

app.MapFallbackToFile("index.html");
app.MapCustomizedIdentityApi<ApplicationUser>();
app.MapControllers();               
//app.MapEndpoints();               

app.Run();            
