namespace Aegis.Application.Services.DataProtection
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	using Aegis.Persistence.Contracts;
	using Aegis.Persistence.Entities.Application;

	using Duende.IdentityServer.Validation;

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
		/// The scope factory
		/// </summary>
		private readonly IServiceScopeFactory _scopeFactory;

		/// <summary>
		/// Get the current key id.
		/// </summary>
		public string CurrentKeyId => _keyRing.Keys.ElementAt(_rnd.Next(_keyRing.Count));

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisLookupProtectorKeyRing"/> class.
		/// </summary>
		public AegisLookupProtectorKeyRing(IServiceScopeFactory scopeFactory)
		{
			_scopeFactory = scopeFactory;
			InitializeKeys();
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
				}
			}
		}
	}
}
