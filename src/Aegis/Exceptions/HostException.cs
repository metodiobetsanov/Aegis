#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Exceptions
{
	using System.Runtime.Serialization;

	using Aegis.Constants;

	/// <summary>
	/// Initializer Exception
	/// </summary>
	/// <seealso cref="Exception" />
	[Serializable]
	public class HostException : Exception
	{
		/// <summary>
		/// Gets the debug message.
		/// </summary>
		/// <value>
		/// The debug message.
		/// </value>
		public string? DebugMessage { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="HostException"/> class.
		/// </summary>
		public HostException()
			: base(HostConstants.SomethingWentWrongWithHost) => this.DebugMessage = HostConstants.SomethingWentWrongWithHost;

		/// <summary>
		/// Initializes a new instance of the <see cref="HostException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public HostException(string? message)
			: base(message) => this.DebugMessage = message;

		/// <summary>
		/// Initializes a new instance of the <see cref="HostException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="debugMessage">The debug message.</param>
		public HostException(string? message, string? debugMessage)
			: base(message) => this.DebugMessage = debugMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="HostException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
		public HostException(string? message, Exception? innerException)
			: base(message, innerException) => this.DebugMessage = message;

		/// <summary>
		/// Initializes a new instance of the <see cref="HostException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="debugMessage">The debug message.</param>
		/// <param name="innerException">The inner exception.</param>
		public HostException(string? message, string? debugMessage, Exception? innerException)
			: base(message, innerException) => this.DebugMessage = debugMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="HostException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		protected HostException(SerializationInfo info, StreamingContext context)
			: base(info, context) => this.DebugMessage = info.GetString(nameof(this.DebugMessage))!;

		/// <summary>
		/// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue(nameof(this.DebugMessage), this.DebugMessage);
			base.GetObjectData(info, context);
		}
	}
}
