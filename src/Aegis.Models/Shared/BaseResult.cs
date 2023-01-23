namespace Aegis.Models.Shared
{
	using System.Collections.Generic;

	/// <summary>
	/// Base Result
	/// </summary>
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.BaseResult&gt;" />
	public record BaseResult
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="BaseResult"/> is success.
		/// </summary>
		/// <value>
		///   <c>true</c> if success; otherwise, <c>false</c>.
		/// </value>
		/// 
		public bool Success { get; init; }

		/// <summary>
		/// Gets or sets the errors.
		/// </summary>
		/// <value>
		/// The errors.
		/// </value>
		public List<KeyValuePair<string, string>> Errors { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BaseResult"/> class.
		/// </summary>
		public BaseResult()
			=> this.Errors = new List<KeyValuePair<string, string>>();
	}
}
