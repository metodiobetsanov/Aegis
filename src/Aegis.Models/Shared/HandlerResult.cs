#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Models.Shared
{
	/// <summary>
	/// Handler Result
	/// </summary>
	/// <seealso cref="Aegis.Models.Shared.BaseResult" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.BaseResult&gt;" />
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.HandlerResult&gt;" />
	public sealed record HandlerResult : BaseResult
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static HandlerResult Succeeded()
			=> new HandlerResult { Success = true };

		/// <summary>
		/// Creates the failed result.
		/// </summary>
		public static HandlerResult Failed()
			=> new HandlerResult();

		/// <summary>
		/// Initializes a new instance of the <see cref="HandlerResult"/> class.
		/// </summary>
		public HandlerResult() : base() { }
	}
}
