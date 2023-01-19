namespace Aegis.Application.Initializers
{
	using Aegis.Application.Constants;
	using Aegis.Application.Contracts.IInitializers;
	using Aegis.Application.Exceptions;
	using Aegis.Persistence;

	using Duende.IdentityServer.EntityFramework.DbContexts;

	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;

	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Database Initializer
	/// </summary>
	/// <seealso cref="Contracts.IInitializers.IInitializer" />
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
		/// Gets a value indicating whether this <see cref="IInitializer" /> is initialized.
		/// </summary>
		/// <value>
		///   <c>true</c> if initialized; otherwise, <c>false</c>.
		/// </value>
		public bool Initialized { get; private set; } = false;

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
			if (!this.Initialized)
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

						this.Initialized = true;
						_logger.LogInformation("{ApplicationName} Database Initializer was successful: {Initialized}.", ApplicationConstants.ApplicationName, this.Initialized);
					}
				}
				catch (Exception ex)
				{
					this.Initialized = false;
					this._logger.LogError(ex, "Database Initializer Error: {message}", ex.Message);

					throw new InitializerException(
						"Database Initializer Error!",
						$"Database Initializer Error: {ex.Message}", ex);
				}
			}
		}
	}
}
