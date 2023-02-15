#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Models.Shared
{
	using System.Collections.Generic;

	/// <summary>
	/// Query Result
	/// </summary>
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Shared.QueryResult&gt;" />
	public record QueryResult<TResult>
	{
		/// <summary>
		/// Creates the success result.
		/// </summary>
		public static QueryResult<TResult> Succeeded(TResult? result)
			=> new QueryResult<TResult> { Success = true, Result = result };

		/// <summary>
		/// Creates the failed result.
		/// </summary>
		public static QueryResult<TResult> Failed()
			=> new QueryResult<TResult>();

		/// <summary>
		/// Gets the result.
		/// </summary>
		/// <value>
		/// The result.
		/// </value>
		public TResult? Result { get; init; }

		/// <summary>
		/// Gets a value indicating whether this <see cref="QueryResult"/> is success.
		/// </summary>
		/// <value>
		///  <c>true</c> if success; otherwise, <c>false</c>.
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
		/// Initializes a new instance of the <see cref="QueryResult"/> class.
		/// </summary>
		public QueryResult()
			=> this.Errors = new List<KeyValuePair<string, string>>();

		/// <summary>
		/// Adds the error.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="code">The code.</param>
		public void AddError(string message, string? code = null)
		{
			if (!this.Success)
			{
				this.Errors.Add(new KeyValuePair<string, string>(code ?? "", message));
			}
		}
	}
}
