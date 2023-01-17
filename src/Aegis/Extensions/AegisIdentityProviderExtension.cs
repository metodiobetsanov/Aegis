namespace Aegis.Extensions
{
	using Aegis.Application.Contracts;
	using Aegis.Application.Services.DataProtection;
	using Aegis.Application.Validators.Application.Settings;
	using Aegis.Exceptions;
	using Aegis.Models.Settings;
	using Aegis.Persistence;
	using Aegis.Persistence.Entities.IdentityProvider;

	using FluentValidation.Results;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;

	using Serilog;

	/// <summary>
	/// Identity Provider Extension
	/// </summary>
	internal static class AegisIdentityProviderExtension
	{
		/// <summary>
		/// Adds the Aegis Identity Provider components.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="logger">The logger.</param>
		/// <returns></returns>
		internal static WebApplicationBuilder AddAegisIdentityProvider(this WebApplicationBuilder builder, ILogger logger)
		{
			logger.Information("Building Aegis Identity Provider.");

			// Add Identity Provider Settings
			logger.Information("Aegis Identity Provider: adding settings.");
			IdentityProviderSettings? identityProviderSettings = builder.Configuration.GetSection(IdentityProviderSettings.Section).Get<IdentityProviderSettings>();

			if (identityProviderSettings is null)
			{
				throw new HostException($"Missing Configuration Section: {IdentityProviderSettings.Section}");
			}

			IdentityProviderSettingsValidator validator = new IdentityProviderSettingsValidator();
			ValidationResult validatioNresults = validator.Validate(identityProviderSettings);

			if (!validatioNresults.IsValid)
			{
				foreach (ValidationFailure error in validatioNresults.Errors)
				{
					logger.Error(error.ErrorMessage);
				}

				throw new HostException($"Validation of Configuration Section {IdentityProviderSettings.Section} failed!");
			}

			builder.Services
				.AddSingleton<IdentityProviderSettings>(identityProviderSettings);

			logger.Information("Aegis Identity Provider: adding DB context.");
			string migrationAssembly = typeof(IAegisPersistenceAssembly).Assembly.GetName().Name!.ToString();

			builder.Services
				 .AddDbContext<AegisIdentityDbContext>(b =>
				 {
					 b.UseLazyLoadingProxies();
					 b.UseNpgsql(
						 builder.Configuration.GetConnectionString("IdentityProviderDatabase"),
						 npq =>
							 npq.MigrationsAssembly(migrationAssembly));
				 });

			// Identity Provider
			logger.Information("Aegis Identity Provider: adding .Net Identity.");
			builder.Services
				.AddHttpContextAccessor()
				.AddIdentity<AegisUser, AegisRole>(options =>
				{
					// SignIn options
					options.SignIn.RequireConfirmedEmail = true;
					options.SignIn.RequireConfirmedAccount = true;

					// User options
					options.User.RequireUniqueEmail = true;

					// Lockout options
					options.Lockout.AllowedForNewUsers = true;
					options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
					options.Lockout.MaxFailedAccessAttempts = 5;

					// Password options
					options.Password.RequireNonAlphanumeric = true;
					options.Password.RequireUppercase = true;
					options.Password.RequireLowercase = true;
					options.Password.RequireDigit = true;
					options.Password.RequiredLength = 8;

					// Protect Personal Data
					options.Stores.ProtectPersonalData = true;
					options.Stores.MaxLengthForKeys = 128;
				})
				.AddEntityFrameworkStores<AegisIdentityDbContext>()
				.AddDefaultTokenProviders();

			logger.Information("Aegis Identity Provider: adding .Net Identity Protection.");
			builder.Services.AddSingleton<IPersonalDataProtector, AegisPersonalDataProtector>();
			builder.Services.AddSingleton<ILookupProtector, AegisLookupProtector>();
			builder.Services.AddSingleton<ILookupProtectorKeyRing, AegisLookupProtectorKeyRing>();

			logger.Information("Aegis Identity Provider: configure Identity Cookie.");
			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/auth/signin";
				options.LogoutPath = "/auth/signout";
				options.AccessDeniedPath = "/access-denied";
			});

			return builder;
		}
	}
}
