namespace Aegis.Models.Settings
{
	/// <summary>
	/// App Settings
	/// </summary>
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Settings.AppSettings&gt;" />
	public sealed record AppSettings
	{
		/// <summary>
		/// The section
		/// </summary>
		public static readonly string Section = nameof(AppSettings);

		/// <summary>
		/// Gets the public domain.
		/// </summary>
		/// <value>
		/// The public domain.
		/// </value>
		public string PublicDomain { get; init; } = default!;

		/// <summary>
		/// Gets the data protection certificate location.
		/// </summary>
		/// <value>
		/// The data protection certificate location.
		/// </value>
		public string DataProtectionCertificateLocation { get; init; } = default!;

		/// <summary>
		/// Gets the data protection certificate password.
		/// </summary>
		/// <value>
		/// The data protection certificate password.
		/// </value>
		public string DataProtectionCertificatePassword { get; init; } = default!;

		/// <summary>
		/// Initializes a new instance of the <see cref="AppSettings"/> class.
		/// </summary>
		public AppSettings()
		{

		}
	}
}
