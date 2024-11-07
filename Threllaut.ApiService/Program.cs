using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Threllaut.Data;
using Threllaut.ApiService.Database.Contexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSqlServer<IdentityDbContext>(builder.Configuration.GetConnectionString("apidatabase"),
    options => options.EnableRetryOnFailure());

builder.Services.AddIdentityApiEndpoints<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddAuthorization();

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails()
    .AddCors();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
    await scope.ServiceProvider.GetRequiredService<IdentityDbContext>()
        .Database.MigrateAsync();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/weatherforecast", ([FromServices] TimeProvider timeProvider) => Enumerable.Range(1, 5)
    .Select(index => new WeatherForecast(DateOnly.FromDateTime(timeProvider.GetLocalNow().AddDays(index).DateTime),
        Random.Shared.Next(-20, 55),
        _summaries[Random.Shared.Next(_summaries.Length)]
    )).ToAsyncEnumerable())
    .RequireCors(policy => policy.SetIsOriginAllowed(_ => true)
        .WithMethods(HttpMethods.Get)
        .AllowAnyHeader()
        .AllowCredentials());

app.MapDefaultEndpoints()
    .MapIdentityApi<ApplicationUser>();

await app.RunAsync();

public static partial class Program
{
    private static readonly string[] _summaries = [
        "Freezing", "Bracing", "Chilly", "Cool", "Mild",
        "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

    private record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(this.TemperatureC / 0.5556);
    }
}
