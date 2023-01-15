namespace Aegis.Application.Initializers
{
	using System.Text;

	using Aegis.Application.Constants;
	using Aegis.Application.Contracts.Application;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Helpers;
	using Aegis.Persistence.Contracts;
	using Aegis.Persistence.Entities.Application;

	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Encryption Initializer
	/// </summary>
	/// <seealso cref="Chimera.Services.Abstractions.IServiceInitializer" />
	public sealed class EncryptionInitializer : IInitializer
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<EncryptionInitializer> _logger;

		/// <summary>
		/// The scope factory
		/// </summary>
		private readonly IServiceScopeFactory _scopeFactory;

		/// <summary>
		/// The initialized
		/// </summary>
		private bool _initialized = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="EncryptionInitializer"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="scopeFactory">The scope factory.</param>
		public EncryptionInitializer(ILogger<EncryptionInitializer> logger, IServiceScopeFactory scopeFactory)
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
						_logger.LogInformation("Security Initializer: Setting up {ApplicationName} Initial Personal Data Protection Key.", ApplicationConstants.ApplicationName);
						IAegisContext context = scope.ServiceProvider.GetRequiredService<IAegisContext>();

						if (!context.PersonalDataProtectionKeys.GetEntities().Any())
						{
							string rndString = IdentityProviderHelper.GenerateRandomPassword();
							string key = Convert.ToBase64String(Encoding.UTF8.GetBytes(rndString));
							string keyHash = key.GetHashCode().ToString();

							context.PersonalDataProtectionKeys.Create(new PersonalDataProtectionKey
							{
								Id = Guid.NewGuid(),
								Key = key,
								KeyHash = keyHash,
								ExpiresOn = DateTime.UtcNow.AddDays(15)
							});

							await context.SaveChangesAsync();
						}
					}

					_logger.LogInformation("{ApplicationName} Initial Security is Initialized.", ApplicationConstants.ApplicationName);
					_initialized = true;
				}
				catch (Exception ex) when (ex is not InitializerException)
				{
					throw new InitializerException(
						"Security Initializer Error!",
						$"Security Initializer Error: {ex.Message}", ex);
				}
			}
		}
	}
}
