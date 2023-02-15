namespace Aegis.Core.Hubs
{
	using Microsoft.AspNetCore.SignalR;

	/// <summary>
	/// Aegis Hub
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.SignalR.Hub" />
	public sealed class AegisHub : Hub
	{
		/// <summary>
		/// Joins the room.
		/// </summary>
		/// <param name="roomName">Name of the room.</param>
		/// <returns></returns>
		public Task JoinRoom(string roomName)
			=> this.Groups.AddToGroupAsync(this.Context.ConnectionId, roomName);

		/// <summary>
		/// Leaves the room.
		/// </summary>
		/// <param name="roomName">Name of the room.</param>
		/// <returns></returns>
		public Task LeaveRoom(string roomName)
			=> this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, roomName);
	}
}
