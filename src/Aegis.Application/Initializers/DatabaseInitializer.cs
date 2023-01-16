namespace Aegis.Application.Initializers
{
	using Aegis.Application.Constants;
	using Aegis.Application.Contracts.Application;
	using Aegis.Application.Exceptions;
	using Aegis.Persistence;

	using Duende.IdentityServer.EntityFramework.DbContexts;

	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;

	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Database Initializer
	/// </summary>
	/// <seealso cref="Contracts.Application.IInitializer" />
	public class DatabaseInitializer : IInitializer
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<DatabaseInitializer> _logger;

		/// <summary>
		/// The scope factory
		/// </summary>
		private readonly IServiceScopeFactory _scopeFactory;

		/// <summary>
		/// The initialized
		/// </summary>
		private bool _initialized = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="DatabaseInitializer"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="scopeFactory">The scope factory.</param>
		public DatabaseInitializer(ILogger<DatabaseInitializer> logger, IServiceScopeFactory scopeFactory)
		{
			_logger = logger;
			_scopeFactory = scopeFactory;
		}

		/// <summary>
		/// Starting the initializing of the service.
		/// </summary>
		public async Task Initialize()
		{
			if (!_initialized)
			{
				try
				{
					using (IServiceScope scope = _scopeFactory.CreateScope())
					{
						_logger.LogInformation("Database Initializer: Migrating {ApplicationName} DBs.", ApplicationConstants.ApplicationName);

						_logger.LogInformation("Database Initializer: Migrating SecureDbContext.");
						await scope.ServiceProvider.GetService<SecureDbContext>()!.Database.MigrateAsync();

						_logger.LogInformation("Database Initializer: Migrating ApplicationIdentityDbContext.");
						await scope.ServiceProvider.GetService<AegisIdentityDbContext>()!.Database.MigrateAsync();

						_logger.LogInformation("Database Initializer: Migrating ConfigurationDbContext.");
						await scope.ServiceProvider.GetService<ConfigurationDbContext>()!.Database.MigrateAsync();

						_logger.LogInformation("Database Initializer: Migrating PersistedGrantDbContext.");
						await scope.ServiceProvider.GetService<PersistedGrantDbContext>()!.Database.MigrateAsync();

						_initialized = true;
						_logger.LogInformation("{ApplicationName} Database Initializer was successful: {_initialized}.", ApplicationConstants.ApplicationName, _initialized);
					}
				}
				catch (Exception ex)
				{
					throw new InitializerException(
						"Database Initializer Error!",
						$"Database Initializer Error: {ex.Message}", ex);
				}
			}
		}
	}
}
