#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Contracts.IInitializers
{
	/// <summary>
	/// Initializer interface
	/// </summary>
	public interface IInitializer
	{
		/// <summary>
		/// Gets a value indicating whether this <see cref="IInitializer"/> is initialized.
		/// </summary>
		/// <value>
		///  <c>true</c> if initialized; otherwise, <c>false</c>.
		/// </value>
		public bool Initialized { get; }

		/// <summary>
		/// Run the initializer.
		/// </summary>
		/// <returns></returns>
		Task Initialize();
	}
}
