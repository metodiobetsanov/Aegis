namespace Aegis.Models.Settings
{
	/// <summary>
	/// SendGrid Settings
	/// </summary>
	public sealed record SendGridSettings
	{
		/// <summary>
		/// The section
		/// </summary>
		public const string Section = nameof(SendGridSettings);

		/// <summary>
		/// Gets or sets the API key.
		/// </summary>
		/// <value>
		/// The API key.
		/// </value>
		public string ApiKey { get; set; } = default!;

		/// <summary>
		/// Gets or sets the verification code template.
		/// </summary>
		/// <value>
		/// The verification code template.
		/// </value>
		public string ResetPasswordTemplate { get; set; } = default!;

		/// <summary>
		/// Gets or sets the email confirmation template.
		/// </summary>
		/// <value>
		/// The email confirmation template.
		/// </value>
		public string EmailConfirmationTemplate { get; set; } = default!;

		/// <summary>
		/// Gets or sets the verification code template.
		/// </summary>
		/// <value>
		/// The verification code template.
		/// </value>
		public string VerificationCodeTemplate { get; set; } = default!;

		/// <summary>
		/// Initializes a new instance of the <see cref="SendGridSettings"/> class.
		/// </summary>
		public SendGridSettings()
		{

		}
	}
}
