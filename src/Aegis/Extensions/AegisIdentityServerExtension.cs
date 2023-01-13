namespace Aegis.Extensions
{
	using Aegis.Application.Contracts;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.EntityFrameworkCore;

	/// <summary>
	/// Identity Server Extension
	/// </summary>
	internal static class AegisIdentityServerExtension
	{
		/// <summary>
		/// Adds the Aegis Identity Server components.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns></returns>
		internal static WebApplicationBuilder AddAegisIdentityServer(this WebApplicationBuilder builder)
		{
			string migrationAssembly = typeof(IAegisPersistenceAssembly).Assembly.GetName().Name!.ToString();

			// Identity Server
			builder.Services
				.AddIdentityServer(options =>
				{
					// Events
					options.Events.RaiseErrorEvents = true;
					options.Events.RaiseInformationEvents = true;
					options.Events.RaiseFailureEvents = true;
					options.Events.RaiseSuccessEvents = true;

					// see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
					options.EmitStaticAudienceClaim = true;
				})
				.AddServerSideSessions()
				.AddConfigurationStore(
					options =>
					options.ConfigureDbContext = b =>
						b.UseNpgsql(
							builder.Configuration.GetConnectionString("DefaultConnection"),
							builder =>
								builder.MigrationsAssembly(migrationAssembly)))
				.AddOperationalStore(
					options =>
					options.ConfigureDbContext = b =>
						b.UseNpgsql(
							builder.Configuration.GetConnectionString("DefaultConnection"),
							builder =>
								builder.MigrationsAssembly(migrationAssembly)))
				.AddAspNetIdentity<AegisUser>();

			return builder;
		}

		/// <summary>
		/// Uses the aegis identity server.
		/// </summary>
		/// <param name="app">The application.</param>
		/// <returns></returns>
		internal static WebApplication UseAegisIdentityServer(this WebApplication app)
		{
			app.UseIdentityServer();
			app.UseAuthorization();

			return app;
		}
	}
}
