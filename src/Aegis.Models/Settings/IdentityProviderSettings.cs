namespace Aegis.Models.Settings
{
	using Aegis.Models.Enums;

	/// <summary>
	/// Identity Provider Settings
	/// </summary>
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Settings.IdentityProviderSettings&gt;" />
	public sealed record IdentityProviderSettings
	{
		/// <summary>
		/// The section
		/// </summary>
		public static readonly string Section = nameof(IdentityProviderSettings);

		/// <summary>
		/// Gets the type of the lookup protector key ring.
		/// </summary>
		/// <value>
		/// The type of the lookup protector key ring.
		/// </value>
		public LookupProtectorKeyRingType? LookupProtectorKeyRingType { get; init; }

		/// <summary>
		/// Gets the lookup protector encryption derivation password.
		/// </summary>
		/// <value>
		/// The lookup protector encryption derivation password.
		/// </value>
		public string LookupProtectorEncryptionDerivationPassword { get; init; }

		/// <summary>
		/// Gets the lookup protector encryption derivation password.
		/// </summary>
		/// <value>
		/// The lookup protector encryption derivation password.
		/// </value>
		public string LookupProtectorSigningDerivationPassword { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AppSettings"/> class.
		/// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public IdentityProviderSettings() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	}
}
