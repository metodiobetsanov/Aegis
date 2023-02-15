#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

using Aegis.Core.Constants;
using Aegis.Core.Hubs;
using Aegis.Extensions;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Sensitive;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Serilog Logger
Logger logger = new LoggerConfiguration()
	.Enrich.WithSensitiveDataMasking(options =>
		options.MaskProperties.AddRange(ApplicationConstants.ProtectedFields))
	.ReadFrom.Configuration(builder.Configuration)
	.CreateLogger();

try
{
	logger.Information("Building Aegis Identity Server.");

	// Add Logging
	builder.Logging
		.ClearProviders();

	builder.Host
		.UseSerilog(logger);

	// Add Aegis
	builder.AddAegisApplication(logger);
	builder.AddAegisIdentityProvider(logger);
	builder.AddAegisIdentityServer(logger);

	// Add Forwarded Header
	builder.Services
		.Configure<ForwardedHeadersOptions>(options
			=> options.ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedProto);

	// Add Health Checks
	builder.Services.AddHealthChecks()
		.AddCheck("Server", () => HealthCheckResult.Healthy(), new[] { "self", "appHealth" });

	// Application
	WebApplication app = builder.Build();

	// Use Aegis
	app.UseAegisApplication();

	app.UseAegisIdentityServer();

	app.MapHub<AegisHub>("/hub");

	app.MapHealthChecks("/health/live", new HealthCheckOptions
	{
		Predicate = r => r.Tags.Contains("self"),
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});

	app.MapHealthChecks("/health/ready", new HealthCheckOptions
	{
		Predicate = r => r.Tags.Contains("appHealth"),
		ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
	});

	app.MapAreaControllerRoute(
		name: ApplicationConstants.AdminArea + "Route",
		areaName: ApplicationConstants.AdminArea,
		pattern: ApplicationConstants.AdminArea + "/{controller=Dashboard}/{action=Index}/{id?}");

	app.MapAreaControllerRoute(
			name: ApplicationConstants.UserArea + "Route",
			areaName: ApplicationConstants.UserArea,
			pattern: ApplicationConstants.UserArea + "/{controller=Profile}/{action=Index}/{id?}");

	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Index}/{id?}");

	logger.Information("Starting Aegis Identity Server.");
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