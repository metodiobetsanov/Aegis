namespace Aegis.Application.Constants
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
	}
}
