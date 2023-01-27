namespace Aegis.Models.Authentication
{
	using System;

	using Aegis.Models.Shared;

	/// <summary>
	/// Get User Locked Time Query Result
	/// </summary>
	/// <seealso cref="Aegis.Models.Shared.BaseResult" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Authentication.GetUserLockedTimeQueryResult&gt;" />
	public sealed record GetUserLockedTimeQueryResult : BaseResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static GetUserLockedTimeQueryResult Succeeded(DateTimeOffset? lockedTill)
			=> new GetUserLockedTimeQueryResult { Success = true, LockedTill = lockedTill };

		/// <summary>
		/// Creates the failed result.
		/// </summary>
		public static GetUserLockedTimeQueryResult Failed()
			=> new GetUserLockedTimeQueryResult();

		/// <summary>
		/// Gets the locked till.
		/// </summary>
		/// <value>
		/// The locked till.
		/// </value>
		public DateTimeOffset? LockedTill { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInQueryResult"/> class.
		/// </summary>
		/// <param name="success">if set to <c>true</c> [succeeded].</param>
		public GetUserLockedTimeQueryResult() : base() { }
	}
}
