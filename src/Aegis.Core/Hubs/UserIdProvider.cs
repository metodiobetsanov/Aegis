namespace Aegis.Core.Hubs
{
	using Microsoft.AspNetCore.SignalR;

	/// <summary>
	/// User Id Provider for SignalR
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.SignalR.IUserIdProvider" />
	public sealed class UserIdProvider : IUserIdProvider
	{
		/// <summary>
		/// Gets the user ID for the specified connection.
		/// </summary>
		/// <param name="connection">The connection to get the user ID for.</param>
		/// <returns>
		/// The user ID for the specified connection.
		/// </returns>
		public string? GetUserId(HubConnectionContext connection)
			=> connection.User?.Identity?.Name;
	}
}
