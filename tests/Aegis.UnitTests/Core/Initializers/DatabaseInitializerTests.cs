namespace Aegis.UnitTests.Core.Initializers
{
	using Duende.IdentityServer.EntityFramework.DbContexts;
	using Duende.IdentityServer.EntityFramework.Storage;

	using global::Aegis.Core.Contracts;
	using global::Aegis.Core.Exceptions;
	using global::Aegis.Core.Initializers;
	using global::Aegis.Persistence;

	public class DatabaseInitializerTests
	{
		private readonly Mock<IServiceScopeFactory> _scf;
		private readonly Mock<ILogger<DatabaseInitializer>> _logger = new Mock<ILogger<DatabaseInitializer>>();

		public DatabaseInitializerTests()
		{
			string migrationAssembly = typeof(IAegisPersistenceAssembly).Assembly.GetName().Name!.ToString();
			ServiceCollection services = new ServiceCollection();

			services.AddDbContext<SecureDbContext>(options =>
				options.UseSqlite("DataSource=./test.db", sql => sql.MigrationsAssembly(migrationAssembly)));

			services.AddDbContext<AegisIdentityDbContext>(options =>
				options.UseSqlite("DataSource=./test.db", sql => sql.MigrationsAssembly(migrationAssembly)));

			services.AddConfigurationDbContext<ConfigurationDbContext>(options => options.ConfigureDbContext = b =>
				b.UseSqlite("DataSource=./test.db", sql => sql.MigrationsAssembly(migrationAssembly)));

			services.AddOperationalDbContext<PersistedGrantDbContext>(options => options.ConfigureDbContext = b =>
				b.UseSqlite("DataSource=./test.db", sql => sql.MigrationsAssembly(migrationAssembly)));

			services.AddScoped<IDataProtectionProvider>(x => new Mock<IDataProtectionProvider>().Object);
			services.AddSingleton<IPersonalDataProtector>(x => new Mock<IPersonalDataProtector>().Object);
			services.AddSingleton<ILookupProtector>(x => new Mock<ILookupProtector>().Object);
			services.AddSingleton<ILookupProtectorKeyRing>(x => new Mock<ILookupProtectorKeyRing>().Object);

			_scf = Helper.GetServiceScopeFactoryMock(services);
		}

		[Fact]
		public void Initialize_ShouldBeTrue()
		{
			// Arrange
			DatabaseInitializer initializer = new DatabaseInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			initializer.Initialize().GetAwaiter().GetResult();

			// Assert
			initializer.Initialized.ShouldBeTrue();
		}

		[Fact]
		public void Initialize_ShouldBeFalse_OnException()
		{
			// Arrange
			_scf.Setup(x => x.CreateScope()).Throws(new Exception());

			DatabaseInitializer initializer = new DatabaseInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			Exception exception = Record.Exception(() => initializer.Initialize().GetAwaiter().GetResult());

			// Assert
			initializer.Initialized.ShouldBeFalse();
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<InitializerException>();
		}
	}
}
