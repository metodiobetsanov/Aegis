using Aegis.Application.Constants;
using Aegis.Application.Contracts;
using Aegis.Application.Contracts.Application;
using Aegis.Models.Settings;

using HealthChecks.UI.Client;

using MediatR;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Sensitive;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Serilog
Logger logger = new LoggerConfiguration()
    .Enrich.WithSensitiveDataMasking(options =>
        options.MaskProperties.AddRange(Helper.ProtectedFields))
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

try
{
    logger.Information("Building Chimera Identity Server.");

    // Logging
    builder.Logging
        .ClearProviders();

    builder.Host
        .UseSerilog(logger);

    // MVC
    builder.Services
        .AddMvc()
        .AddNewtonsoftJson();

    // Settings
    AppSettings appSettings = builder.Configuration.GetSection(AppSettings.Section).Get<AppSettings>();

    builder.Services
        .AddSingleton<AppSettings>(appSettings);

    // Services
    builder.Services.AddMediatR(typeof(IAegisApplicationAssembly).Assembly);

    // Forwared Header
    builder.Services
        .Configure<ForwardedHeadersOptions>(options
            => options.ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto);

    // Health Checks
    builder.Services.AddHealthChecks()
        .AddCheck("Service", () => HealthCheckResult.Healthy(), new[] { "self", "appHealth" });

    // Application
    WebApplication app = builder.Build();

    app.UsePathBase(new PathString(builder.Configuration.GetValue<string>("BASEPATH", "/")));

    using (IServiceScope scope = app.Services.CreateScope())
    {
        IEnumerable<IInitializer> initializers = scope.ServiceProvider.GetServices<IInitializer>();

        foreach (IInitializer initializer in initializers)
        {
            initializer.Initialize().GetAwaiter().GetResult();
        }
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Error");
    }

    app.UseStaticFiles();
    app.UseForwardedHeaders();

    app.UseSerilogRequestLogging();
    app.UseRouting();

    app.Use(async (ctx, next) =>
    {
        ctx.Request.Scheme = "https";
        await next();
    });

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("self"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
        {
            Predicate = r => r.Tags.Contains("appHealth"),
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
    });

    logger.Information("Starting Chimera Identity Server.");
    app.Run();

    return 0;
}
catch (Exception ex)
{
    logger.Fatal(ex, "An unexpected error was thrown!");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}