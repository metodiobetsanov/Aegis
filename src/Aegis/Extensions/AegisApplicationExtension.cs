namespace Aegis.Extensions
{
	using System.Security.Cryptography.X509Certificates;

	using Aegis.Application.Constants;
	using Aegis.Application.Contracts;
	using Aegis.Application.Contracts.Application;
	using Aegis.Application.Validators.Application.Settings;
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

			AppSettingsValidator validator = new AppSettingsValidator();
			ValidationResult validatioNresults = validator.Validate(appSettings);

			if (!validatioNresults.IsValid)
			{
				foreach (ValidationFailure error in validatioNresults.Errors)
				{
					logger.Error(error.ErrorMessage);
				}

				throw new HostException($"Validation of Configuration Section {AppSettings.Section} failed!");
			}

			builder.Services
				.AddSingleton<AppSettings>(appSettings);

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

				app.Use(async (ctx, next) =>
				{
					ctx.Request.Scheme = "https";
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
