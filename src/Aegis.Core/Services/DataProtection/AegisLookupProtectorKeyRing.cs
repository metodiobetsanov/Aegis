namespace Aegis.Core.Services.DataProtection
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	using Aegis.Core.Constants;
	using Aegis.Core.Events.Audit.DataProtection;
	using Aegis.Core.Exceptions;
	using Aegis.Core.Helpers;
	using Aegis.Persistence.Contracts;
	using Aegis.Persistence.Entities.Application;

	using Duende.IdentityServer.Validation;

	using MediatR;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.DependencyInjection;

	/// <summary>
	/// Aegis Lookup Protector KeyRing
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Identity.ILookupProtector" />
	public sealed class AegisLookupProtectorKeyRing : ILookupProtectorKeyRing
	{
		/// <summary>
		/// The random
		/// </summary>
		private readonly Random _rnd = new Random();

		/// <summary>
		/// The key ring
		/// </summary>
		private readonly Dictionary<string, string> _keyRing = new Dictionary<string, string>();

		/// <summary>
		/// The active keys
		/// </summary>
		private readonly List<string> _activeKeys = new List<string>();

		/// <summary>
		/// The scope factory
		/// </summary>
		private readonly IServiceScopeFactory _scopeFactory;

		/// <summary>
		/// Get the current key id.
		/// </summary>
		public string CurrentKeyId => _activeKeys.ElementAt(_rnd.Next(0, _keyRing.Count - 1));

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisLookupProtectorKeyRing"/> class.
		/// </summary>
		public AegisLookupProtectorKeyRing(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
			this.InitializeKeys();
		}

		/// <summary>
		/// Gets the <see cref="string"/> with the specified key identifier.
		/// </summary>
		/// <value>
		/// The <see cref="string"/>.
		/// </value>
		/// <param name="keyId">The key identifier.</param>
		/// <returns></returns>
		public string this[string keyId]
			=> _keyRing[keyId];

		/// <summary>
		/// Return all of the key ids.
		/// </summary>
		/// <returns>
		/// All of the key ids.
		/// </returns>
		public IEnumerable<string> GetAllKeyIds()
			=> _keyRing.Select(x => x.Key);

		/// <summary>
		/// Initializes a new instance of the <see cref="T:System.Object" /> class.
		/// </summary>
		/// <returns></returns>
		private void InitializeKeys()
		{
			using (IServiceScope scope = _scopeFactory.CreateScope())
			using (IAegisContext context = scope.ServiceProvider.GetRequiredService<IAegisContext>())
			{
				IEnumerable<PersonalDataProtectionKey> keys = context.PersonalDataProtectionKeys.GetEntities();

				foreach (PersonalDataProtectionKey key in keys)
				{
					_keyRing.Add(key.KeyHash, key.Key);

					if (key.ExpiresOn >= DateTime.UtcNow.AddDays(3))
					{
						_activeKeys.Add(key.KeyHash);
					}
				}

				if (_activeKeys.Count == 0)
				{
					IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
					Guid keyId = Guid.NewGuid();

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
							ExpiresOn = DateTime.UtcNow.AddDays(15)
						};

						context.PersonalDataProtectionKeys.Create(personalDataProtectionKey);
						int result = context.SaveChanges();

						if (result > 0)
						{
							_keyRing.Add(personalDataProtectionKey.KeyHash, personalDataProtectionKey.Key);
							_activeKeys.Add(personalDataProtectionKey.KeyHash);

							mediator.Publish(new CreateLookupProtectionKeySucceededAuditEvent(
							keyId, "AegisLookupProtectorKeyRing: add a short-lived lookup protection key.", true))
								.GetAwaiter().GetResult();
						}
						else
						{
							mediator.Publish(new CreateLookupProtectionKeyFailedAuditEvent(
								keyId, "AegisLookupProtectorKeyRing: add a short-lived lookup protection key.", true))
								.GetAwaiter().GetResult();

							throw new ServiceException(
								ServiceConstants.SomethingWentWrong,
								"Failed to execute InitializeKeys in AegisLookupProtectorKeyRing!");
						}
					}
					catch (Exception ex) when (ex is not ServiceException)
					{
						mediator.Publish(new CreateLookupProtectionKeyFailedAuditEvent(
							keyId, "AegisLookupProtectorKeyRing: add a short-lived lookup protection key.", true))
							.GetAwaiter().GetResult();

						throw new ServiceException(
								ServiceConstants.SomethingWentWrong,
							$"Security Initializer Error: {ex.Message}", ex);
					}
				}
			}
		}
	}
}
