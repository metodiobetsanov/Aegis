#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Constants
{
	using System.Collections.Immutable;

	/// <summary>
	/// Helper
	/// </summary>
	public static class ApplicationConstants
	{
		/// <summary>
		/// The application name
		/// </summary>
		public static readonly string ApplicationName = "Aegis Identity Server";

		/// <summary>
		/// The protected fields
		/// </summary>
		public static readonly ImmutableList<string> ProtectedFields = ImmutableList.Create<string>("PASSWORD");

		/// <summary>
		/// Initializer Something went wrong message
		/// </summary>
		public static readonly string InitializerSomethingWentWrong = "Something went wrong. Unable to execute Initializer!";

		/// <summary>
		/// The application name
		/// </summary>
		public const string ApplicationEmail = "no-replay@mnb.software";

		/// <summary>
		/// The Admin area
		/// </summary>
		public const string AdminArea = "Admin";

		/// <summary>
		/// The User area
		/// </summary>
		public const string UserArea = "User";
	}
}
