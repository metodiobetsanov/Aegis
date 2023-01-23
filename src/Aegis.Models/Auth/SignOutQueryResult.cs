namespace Aegis.Models.Auth
{
	using Aegis.Models.Shared;

	/// <summary>
	/// SignOut Query Result
	/// </summary>
	/// <seealso cref="Aegis.Models.Shared.BaseResult" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Auth.SignOutQueryResult&gt;" />
	public sealed record SignOutQueryResult : BaseResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static SignOutQueryResult Show(bool show)
			=> new SignOutQueryResult { ShowSignoutPrompt = show };

		/// <summary>
		/// Gets a value indicating whether [show signout prompt].
		/// </summary>
		/// <value>
		///   <c>true</c> if [show signout prompt]; otherwise, <c>false</c>.
		/// </value>
		public bool ShowSignoutPrompt { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignOutQueryResult"/> class.
		/// </summary>
		public SignOutQueryResult() : base() { }
	}
}
