namespace Aegis.Extensions
{
	using Aegis.Application.Contracts;
	using Aegis.Application.Contracts.Application;
	using Aegis.Exceptions;
	using Aegis.Models.Settings;

	using MediatR;

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
		/// <param name="builder">The builder.</param>
		/// <returns></returns>
		internal static WebApplicationBuilder AddAegisApplication(this WebApplicationBuilder builder)
		{
			// Add Application Settings
			AppSettings? appSettings = builder.Configuration.GetSection(AppSettings.Section).Get<AppSettings>();

			if (appSettings is null)
			{
				throw new HostException($"Missing Configuration Section: {AppSettings.Section}");
			}

			builder.Services
				.AddSingleton<AppSettings>(appSettings);

			// Add MediatR
			builder.Services
				.AddMediatR(typeof(IAegisApplicationAssembly).Assembly);

			//Add AutoMapper
			builder.Services
				.AddAutoMapper(cfg =>
				{
				});

			// Add Initializers
			builder.Services.Scan(delegate (ITypeSourceSelector s)
			{
				_ = s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies()).AddClasses(delegate (IImplementationTypeFilter c)
				{
					_ = c.AssignableTo(typeof(IInitializer));
				}).AsImplementedInterfaces()
					.WithSingletonLifetime();
			});

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
