using Aspire.Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Threllaut.Data;
using Threllaut.Data.Contexts;
using Threllaut.Shared.Services;
using Threllaut.Web.Components;
using Threllaut.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerDbContext<IdentityDbContext>("database");
builder.AddSqlServerDbContextFactory<ApplicationDbContext>("database");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        microsoftOptions.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"]
            ?? throw new InvalidOperationException("Missing Microsoft Application (client) ID.");
        microsoftOptions.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"]
            ?? throw new InvalidOperationException("Missing Microsoft Secret ID.");
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]
            ?? throw new InvalidOperationException("Missing Google Client ID.");
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
            ?? throw new InvalidOperationException("Missing Google Client Secret.");
    })
    .AddGitHub(githubOptions =>
    {
        githubOptions.ClientId = builder.Configuration["Authentication:Github:ClientId"]
            ?? throw new InvalidOperationException("Missing Github Client ID.");
        githubOptions.ClientSecret = builder.Configuration["Authentication:Github:ClientSecret"]
            ?? throw new InvalidOperationException("Missing Github Client secret.");
    })
    .AddGitLab(gitlabOptions =>
    {
        gitlabOptions.ClientId = builder.Configuration["Authentication:GitLab:ClientId"]
            ?? throw new InvalidOperationException("Missing GitLab Application ID.");
        gitlabOptions.ClientSecret = builder.Configuration["Authentication:GitLab:ClientSecret"]
            ?? throw new InvalidOperationException("Missing GitLab Secret.");
    })
    .AddIdentityCookies();

builder.Services.AddAuthorization();
builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.AddServiceDefaults();

builder.Services.AddMudServices()
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<IFormFactor, FormFactor>();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapStaticAssets();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(Threllaut.Shared._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

await app.RunAsync();

public static partial class Program
{
    public static IHostApplicationBuilder AddSqlServerDbContextFactory<TContext>(this IHostApplicationBuilder builder,
        string connectionName)
    where TContext : DbContext
    {
        const string DefaultConfigSectionName = "Aspire:Microsoft:EntityFrameworkCore:SqlServer";
        string typeSpecificSectionName = $"{DefaultConfigSectionName}:{typeof(TContext).Name}";
        MicrosoftEntityFrameworkCoreSqlServerSettings settings = new();
        var configurationSection = builder.Configuration.GetSection(DefaultConfigSectionName);
        configurationSection.Bind(settings);
        var typeSpecificConfigurationSection = configurationSection.GetSection(typeof(TContext).Name);
        if (typeSpecificConfigurationSection.Exists())
        {
            typeSpecificConfigurationSection.Bind(settings);
        }

        settings.ConnectionString = builder.Configuration.GetConnectionString(connectionName);
        builder.Services.AddDbContextFactory<ApplicationDbContext>(optionsBuilder =>
        {
            if (builder.Environment.IsDevelopment())
                optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlServer(settings.ConnectionString, builder =>
            {
                if (string.IsNullOrWhiteSpace(settings.ConnectionString) && !EF.IsDesignTime)
                {
                    throw new InvalidOperationException($"ConnectionString is missing. It should be provided in 'ConnectionStrings:{connectionName}' "
                        + $"or under the 'ConnectionString' key in '{DefaultConfigSectionName}' "
                        + $"{(!string.IsNullOrEmpty(typeSpecificSectionName) ? "or '{typeSpecificSectionName}' " : string.Empty)}configuration section.");
                }

                // Resiliency:
                // Connection resiliency automatically retries failed database commands
                if (!settings.DisableRetry)
                {
                    builder.EnableRetryOnFailure();
                }

                // The time in seconds to wait for the command to execute.
                if (settings.CommandTimeout.HasValue)
                {
                    builder.CommandTimeout(settings.CommandTimeout);
                }
            });
        });

        if (!settings.DisableTracing)
        {
            /*builder.Services.AddOpenTelemetry().WithTracing(builder =>
            {
                builder.AddInstrumentation(sp =>
                {
                    var sqlOptions = sp.GetRequiredService<IOptionsMonitor<SqlClientTraceInstrumentationOptions>>().Get(name);

                    return new SqlClientInstrumentation(sqlOptions);
                });

                builder.AddSource(SqlActivitySourceHelper.ActivitySourceName);
            });*/
        }

        if (!settings.DisableHealthChecks)
        {
            string key = "Aspire.HealthChecks." + typeof(TContext).Name;
            if (!builder.Properties.ContainsKey(key))
            {
                builder.Properties[key] = true;
                builder.Services.AddHealthChecks()
                    .AddDbContextCheck<TContext>();
            }
        }
        return builder;
    }
}
