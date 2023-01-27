#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Extensions
{
	using System.Security.Cryptography.X509Certificates;

	using Aegis.Core.Constants;
	using Aegis.Core.Constants.Services;
	using Aegis.Core.Contracts;
	using Aegis.Core.Contracts.IInitializers;
	using Aegis.Core.Services;
	using Aegis.Core.Validators.Settings;
	using Aegis.Exceptions;
	using Aegis.Models.Settings;
	using Aegis.Persistence;
	using Aegis.Persistence.Contracts;

	using FluentValidation.Results;

	using MediatR;

	using Microsoft.AspNetCore.DataProtection;
	using Microsoft.EntityFrameworkCore;

	using Scrutor;

	using Serilog;

	/// <summary>
	/// Application Extension
	/// </summary>
	internal static class AegisApplicationExtension
	{
		/// <summary>
		/// Adds the Aegis application components.
		/// </summary>
		/// <param name="builder">The builder.</param
		/// <param name="logger">The logger.</param>
		/// <returns></returns>
		internal static WebApplicationBuilder AddAegisApplication(this WebApplicationBuilder builder, ILogger logger)
		{
			logger.Information("Building Aegis Core.");

			// Add Application Settings
			logger.Information("Aegis Core: adding settings.");
			AppSettings? appSettings = builder.Configuration.GetSection(AppSettings.Section).Get<AppSettings>();

			if (appSettings is null)
			{
				throw new HostException($"Missing Configuration Section: {AppSettings.Section}");
			}

			AppSettingsValidator appSettingsValidator = new AppSettingsValidator();
			ValidationResult appSettingsValidationResults = appSettingsValidator.Validate(appSettings);

			if (!appSettingsValidationResults.IsValid)
			{
				foreach (ValidationFailure error in appSettingsValidationResults.Errors)
				{
					logger.Error(error.ErrorMessage);
				}

				throw new HostException($"Validation of Configuration Section {AppSettings.Section} failed!");
			}

			SendGridSettings? sendGridSettings = builder.Configuration.GetSection(SendGridSettings.Section).Get<SendGridSettings>();

			if (sendGridSettings is null)
			{
				throw new HostException($"Missing Configuration Section: {SendGridSettings.Section}");
			}

			SendGridSettingsValidator sendGridSettingsValidator = new SendGridSettingsValidator();
			ValidationResult sendGridSettingsValidationResults = sendGridSettingsValidator.Validate(sendGridSettings);

			if (!sendGridSettingsValidationResults.IsValid)
			{
				foreach (ValidationFailure error in sendGridSettingsValidationResults.Errors)
				{
					logger.Error(error.ErrorMessage);
				}

				throw new HostException($"Validation of Configuration Section {AppSettings.Section} failed!");
			}

			builder.Services
				.AddSingleton<AppSettings>(appSettings)
				.AddSingleton<SendGridSettings>(sendGridSettings);

			logger.Information("Aegis Core: adding DB context.");
			string migrationAssembly = typeof(IAegisPersistenceAssembly).Assembly.GetName().Name!.ToString();

			builder.Services
				 .AddDbContext<SecureDbContext>(b =>
				 {
					 b.UseLazyLoadingProxies();
					 b.UseNpgsql(
						 builder.Configuration.GetConnectionString("SecureDatabase"),
						 npq =>
							 npq.MigrationsAssembly(migrationAssembly));
				 });

			// Data Protection
			logger.Information("Aegis Core: adding Data Protection.");
			builder.Services
				.AddDataProtection()
				.PersistKeysToDbContext<SecureDbContext>()
				.SetApplicationName(ApplicationConstants.ApplicationName.ToLower().Replace(' ', '_'))
				.SetDefaultKeyLifetime(TimeSpan.FromDays(14))
				.ProtectKeysWithCertificate(
				 new X509Certificate2(appSettings.DataProtectionCertificateLocation, appSettings.DataProtectionCertificatePassword));

			// Add MediatR
			logger.Information("Aegis Core: adding MediatR.");
			builder.Services
				.AddMediatR(typeof(IAegisApplicationAssembly).Assembly);

			logger.Information("Aegis Core: adding AutoMapper.");
			//Add AutoMapper
			builder.Services
				.AddAutoMapper(cfg =>
				{
				});

			logger.Information("Aegis Core: adding Initializers.");
			// Add Initializers
			builder.Services.Scan(delegate (ITypeSourceSelector s)
			{
				_ = s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies()).AddClasses(delegate (IImplementationTypeFilter c)
				{
					_ = c.AssignableTo(typeof(IInitializer));
				}).AsImplementedInterfaces()
					.WithSingletonLifetime();
			});

			logger.Information("Aegis Core: adding Services.");
			builder.Services
				.AddSingleton<IMailSenderService, MailSenderService>()
				.AddScoped<IAegisContext, AegisContext>();

			return builder;
		}

		/// <summary>
		/// Uses the Aegis application components.
		/// </summary>
		/// <param name="app">The application.</param>
		/// <returns></returns>
		internal static WebApplication UseAegisApplication(this WebApplication app)
		{
			// Use Base path
			app.UsePathBase(new PathString(app.Configuration.GetValue<string>("BASEPATH", "/")));

			// Use Initializers
			using (IServiceScope scope = app.Services.CreateScope())
			{
				IEnumerable<IInitializer> initializers = scope.ServiceProvider.GetServices<IInitializer>();

				foreach (IInitializer initializer in initializers)
				{
					initializer.Initialize().GetAwaiter().GetResult();
				}
			}

			// Environment specific
			if (app.Environment.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();

				// simulate a proxy headers
				app.Use(async (ctx, next) =>
				{
					ctx.Request.Headers.Add("X-Forwarded-Proto", "https");
					await next();
				});
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();
			app.UseForwardedHeaders();

			app.UseSerilogRequestLogging();
			app.UseRouting();

			return app;
		}
	}
}
