namespace Aegis.Models.Settings
{
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
		/// Gets the lookup protector encryption derivation password.
		/// </summary>
		/// <value>
		/// The lookup protector encryption derivation password.
		/// </value>
		public string LookupProtectorEncryptionDerivationPassword { get; init; } = default!;

		/// <summary>
		/// Gets the lookup protector encryption derivation password.
		/// </summary>
		/// <value>
		/// The lookup protector encryption derivation password.
		/// </value>
		public string LookupProtectorSigningDerivationPassword { get; init; } = default!;

		/// <summary>
		/// Initializes a new instance of the <see cref="AppSettings"/> class.
		/// </summary>
		public IdentityProviderSettings()
		{

		}
	}
}
