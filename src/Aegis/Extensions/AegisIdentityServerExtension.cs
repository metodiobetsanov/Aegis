namespace Aegis.Extensions
{
	using Aegis.Application.Contracts;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.EntityFrameworkCore;

	using Serilog;

	/// <summary>
	/// Identity Server Extension
	/// </summary>
	internal static class AegisIdentityServerExtension
	{
		/// <summary>
		/// Adds the Aegis Identity Server components.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="logger">The logger.</param>
		/// <returns></returns>
		internal static WebApplicationBuilder AddAegisIdentityServer(this WebApplicationBuilder builder, ILogger logger)
		{
			logger.Information("Building Aegis Identity Server.");

			logger.Information("Aegis Identity Server: adding Duende IS.");
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
					{
						options.DefaultSchema = "aegis-ids";
						options.ConfigureDbContext = b =>
									b.UseNpgsql(
										builder.Configuration.GetConnectionString("IdentityServerDatabase"),
										npq =>
											npq.MigrationsAssembly(migrationAssembly));
					})
				.AddOperationalStore(
					options =>
					{
						options.DefaultSchema = "aegis-ids";
						options.ConfigureDbContext = b =>
							b.UseNpgsql(
								builder.Configuration.GetConnectionString("IdentityServerDatabase"),
								npq =>
									npq.MigrationsAssembly(migrationAssembly));
					})
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
