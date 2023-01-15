namespace Aegis.Extensions
{
	using System.IO;
	using System.Security.Cryptography.X509Certificates;

	using Aegis.Application.Constants;
	using Aegis.Application.Contracts;
	using Aegis.Application.Services.DataProtection;
	using Aegis.Exceptions;
	using Aegis.Models.Enums;
	using Aegis.Models.Settings;
	using Aegis.Persistence;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.DataProtection;
	using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	/// Identity Provider Extension
	/// </summary>
	internal static class AegisIdentityProviderExtension
	{
		/// <summary>
		/// Adds the Aegis Identity Provider components.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns></returns>
		internal static WebApplicationBuilder AddAegisIdentityProvider(this WebApplicationBuilder builder)
		{
			// Add Identity Provider Settings
			IdentityProviderSettings? identityProviderSettings = builder.Configuration.GetSection(IdentityProviderSettings.Section).Get<IdentityProviderSettings>();

			if (identityProviderSettings is null)
			{
				throw new HostException($"Missing Configuration Section: {IdentityProviderSettings.Section}");
			}

			builder.Services
				.AddSingleton<IdentityProviderSettings>(identityProviderSettings);

			string migrationAssembly = typeof(IAegisPersistenceAssembly).Assembly.GetName().Name!.ToString();

			builder.Services
				 .AddDbContext<AegisIdentityDbContext>(b =>
				 {
					 b.UseLazyLoadingProxies();
					 b.UseNpgsql(
						 builder.Configuration.GetConnectionString("IdentityProviderDatabase"),
						 builder =>
							 builder.MigrationsAssembly(migrationAssembly));
				 });

			// Identity Provider
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

			builder.Services.AddSingleton<IPersonalDataProtector, AegisPersonalDataProtector>();
			builder.Services.AddSingleton<ILookupProtector, AegisLookupProtector>();
			builder.Services.AddSingleton<ILookupProtectorKeyRing, AegisLookupProtectorKeyRing>();

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.LoginPath = "/auth/signin";
				options.AccessDeniedPath = "/access-denied";
			});

			return builder;
		}
	}
}
