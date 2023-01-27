#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Initializers
{
	using System.Text;

	using Aegis.Core.Constants;
	using Aegis.Core.Contracts.IInitializers;
	using Aegis.Core.Events.Audit.DataProtection;
	using Aegis.Core.Exceptions;
	using Aegis.Core.Helpers;
	using Aegis.Persistence.Contracts;
	using Aegis.Persistence.Entities.Application;

	using MediatR;

	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Encryption Initializer
	/// </summary>
	/// <seealso cref="Aegis.Services.Abstractions.IServiceInitializer" />
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
		/// Gets a value indicating whether this <see cref="IInitializer" /> is initialized.
		/// </summary>
		/// <value>
		///  <c>true</c> if initialized; otherwise, <c>false</c>.
		/// </value>
		public bool Initialized { get; private set; } = false;

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
			if (!this.Initialized)
			{
				try
				{
					using (IServiceScope scope = _scopeFactory.CreateScope())
					{
						_logger.LogInformation("Encryption Initializer: Setting up {ApplicationName} Initial Personal Data Protection Key.", ApplicationConstants.ApplicationName);
						IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
						IAegisContext context = scope.ServiceProvider.GetRequiredService<IAegisContext>();
						_logger.LogInformation("Encryption Initializer: check if there are any keys.");

						if (!context.PersonalDataProtectionKeys.GetEntities().Any())
						{
							Guid keyId = Guid.NewGuid();
							_logger.LogInformation("Encryption Initializer: no keys found, creating new short-lived key(ID: {keyId}).", keyId);

							try
							{
								string rndString = IdentityProviderHelper.GenerateRandomPassword();
								string key = Convert.ToBase64String(Encoding.UTF8.GetBytes(rndString));
								string keyHash = key.GetHashCode().ToString();

								PersonalDataProtectionKey personalDataProtectionKey = new PersonalDataProtectionKey
								{
									Id = keyId,
									Key = key,
									KeyHash = keyHash,
									ExpiresOn = DateTime.UtcNow.AddDays(3)
								};

								context.PersonalDataProtectionKeys.Create(personalDataProtectionKey);

								int result = await context.SaveChangesAsync();

								if (result > 0)
								{
									this.Initialized = true;
									await mediator.Publish(new CreateLookupProtectionKeySucceededAuditEvent(
									keyId, "Encryption Initializer: add a lookup protection key.", true));
								}
								else
								{
									await mediator.Publish(new CreateLookupProtectionKeyFailedAuditEvent(
										keyId, "Encryption Initializer: add a lookup protection key.", true));
								}
							}
							catch (Exception ex)
							{
								this.Initialized = false;
								this._logger.LogError(ex, "Encryption Initializer Error: {message}", ex.Message);
								await mediator.Publish(new CreateLookupProtectionKeyFailedAuditEvent(
											keyId, "Encryption Initializer: " + ex.Message, true));

								throw new InitializerException(
									InitializerConstants.SomethingWentWrong,
									$"Encryption Initializer Error: {ex.Message}", ex);
							}

							_logger.LogInformation("{ApplicationName} Encryption Initializer was successful: {Initialized}.", ApplicationConstants.ApplicationName, this.Initialized);
						}
						else
						{
							this.Initialized = true;
						}
					}
				}
				catch (Exception ex) when (ex is not InitializerException)
				{
					this.Initialized = false;
					this._logger.LogError(ex, "Encryption Initializer Error: {message}", ex.Message);

					throw new InitializerException(
						InitializerConstants.SomethingWentWrong,
						$"Encryption Initializer Error: {ex.Message}", ex);
				}
			}
		}
	}
}
