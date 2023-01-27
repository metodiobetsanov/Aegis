#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.Persistence.Entities
{
	using System;

	/// <summary>
	/// Aegis Entity
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		/// A random value that should change whenever an entity is persisted to the store
		/// </summary>
		/// <value>
		/// The concurrency stamp.
		/// </value>
		string? ConcurrencyStamp { get; set; }

		/// <summary>
		/// Gets or sets the created on.
		/// </summary>
		/// <value>
		/// The created on.
		/// </value>
		DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the updated on.
		/// </summary>
		/// <value>
		/// The updated on.
		/// </value>
		DateTime? UpdatedOn { get; set; }
	}
}
