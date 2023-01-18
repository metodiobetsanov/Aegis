namespace Aegis.Models.Shared
{
	/// <summary>
	/// Authentication Result
	/// </summary>
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.AuthenticationResult&gt;" />
	public sealed record AuthenticationResult
	{
		/// <summary>
		/// Gets or sets a value indicating whether the process succeeded.
		/// </summary>
		/// <value>
		///   <c>true</c> if succeeded; otherwise, <c>false</c>.
		/// </value>
		public bool Succeeded { get; init; }

		/// <summary>
		/// Gets a value indicating whether [redirect to action].
		/// </summary>
		/// <value>
		///   <c>true</c> if [redirect to action]; otherwise, <c>false</c>.
		/// </value>
		public bool RedirectToAction { get; init; }

		/// <summary>
		/// Gets the action.
		/// </summary>
		/// <value>
		/// The action.
		/// </value>
		public string? Action { get; init; }

		/// <summary>
		/// Gets the action parameters.
		/// </summary>
		/// <value>
		/// The action parameters.
		/// </value>
		public object? ActionParameters { get; init; }

		/// <summary>
		/// Gets or sets the return URL.
		/// </summary>
		/// <value>
		/// The return URL.
		/// </value>
		public string? ReturnUrl { get; init; }

		/// <summary>
		/// Gets or sets the errors.
		/// </summary>
		/// <value>
		/// The errors.
		/// </value>
		public List<KeyValuePair<string, string>> Errors { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
		/// </summary>
		/// <param name="succeeded">if set to <c>true</c> [succeeded].</param>
		public AuthenticationResult(bool succeeded)
		{
			this.Succeeded = succeeded;
			this.Errors = new List<KeyValuePair<string, string>>();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="actionParameters">The action parameters.</param>
		public AuthenticationResult(string? action, object? actionParameters)
				: this(false)
		{
			this.RedirectToAction = true;
			this.Action = action;
			this.ActionParameters = actionParameters;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationResult"/> class.
		/// </summary>
		/// <param name="returnUrl">The return URL.</param>
		public AuthenticationResult(string returnUrl)
			: this(true)
		{
			this.ReturnUrl = returnUrl;
		}
	}
}
