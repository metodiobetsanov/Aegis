namespace Aegis.Application.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	using Aegis.Application.Constants;
	using Aegis.Application.Contracts;

	/// <summary>
	/// Identity Server Exception
	/// </summary>
	/// <seealso cref="System.Exception" />
	[Serializable]
	public class IdentityServerException : Exception, IAegisException
	{
		/// <summary>
		/// Gets the debug message.
		/// </summary>
		/// <value>
		/// The debug message.
		/// </value>
		public string? DebugMessage { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityServerException"/> class.
		/// </summary>
		public IdentityServerException()
			: base(IdentityServerConstants.SomethingWentWrong) => this.DebugMessage = IdentityServerConstants.SomethingWentWrong;

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityServerException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public IdentityServerException(string? message)
			: base(message) => this.DebugMessage = message;

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityServerException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="debugMessage">The debug message.</param>
		public IdentityServerException(string? message, string? debugMessage)
			: base(message) => this.DebugMessage = debugMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityServerException"/> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified.</param>
		public IdentityServerException(string? message, Exception? innerException)
			: base(message, innerException) => this.DebugMessage = message;

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityServerException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="debugMessage">The debug message.</param>
		/// <param name="innerException">The inner exception.</param>
		public IdentityServerException(string? message, string? debugMessage, Exception? innerException)
			: base(message, innerException) => this.DebugMessage = debugMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityServerException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
		protected IdentityServerException(SerializationInfo info, StreamingContext context)
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
