namespace Aegis.Models.Auth
{
	using Aegis.Models.Shared;

	/// <summary>
	/// Email Confirmation Query
	/// </summary>
	/// <seealso cref="Aegis.Models.Shared.BaseResult" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Auth.EmailConfirmationQueryResult&gt;" />
	public sealed record EmailConfirmationQueryResult : BaseResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static EmailConfirmationQueryResult Succeeded()
			=> new EmailConfirmationQueryResult { Success = true };

		/// <summary>
		/// Creates the failed result.
		/// </summary>
		public static EmailConfirmationQueryResult Failed()
			=> new EmailConfirmationQueryResult();

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailConfirmationQueryResult"/> class.
		/// </summary>
		public EmailConfirmationQueryResult() : base() { }
	}
}
