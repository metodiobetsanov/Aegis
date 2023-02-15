namespace Aegis.Models.Users
{
	using System.Runtime.Serialization;

	/// <summary>
	/// User Simple View Model
	/// </summary>
	/// <seealso cref="System.IEquatable&lt;Aegis.Models.Users.UserViewModel&gt;" />
	[DataContract]
	public record UserViewModel
	{
		/// <summary>
		/// Gets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		[DataMember]
		public string? Id { get; init; }

		/// <summary>
		/// Gets the profile picture URL.
		/// </summary>
		/// <value>
		/// The profile picture URL.
		/// </value>
		[DataMember]
		public string? ProfilePictureURL { get; init; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name.
		/// </value>
		[DataMember]
		public string? FirstName { get; init; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>
		/// The last name.
		/// </value>
		[DataMember]
		public string? LastName { get; init; }

		/// <summary>
		/// Gets the full name.
		/// </summary>
		/// <value>
		/// The full name.
		/// </value>
		[DataMember]
		public string? FullName => $"{this.FirstName} {this.LastName}";

		/// <summary>
		/// Gets the email.
		/// </summary>
		/// <value>
		/// The email.
		/// </value>
		[DataMember]
		public string? Email { get; init; }

		/// <summary>
		/// Gets a value indicating whether [two factor enabled].
		/// </summary>
		/// <value>
		///   <c>true</c> if [two factor enabled]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool TwoFactorEnabled { get; init; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is system user.
		/// </summary>
		/// <value>
		///  <c>true</c> if this instance is system user; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool SystemUser { get; init; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is protected user.
		/// </summary>
		/// <value>
		///  <c>true</c> if this instance is protected user; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool ProtectedUser { get; init; }

		/// <summary>
		/// Gets or sets a value indicating whether [active profile].
		/// </summary>
		/// <value>
		///  <c>true</c> if [active profile]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool ActiveProfile { get; init; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is profile completed.
		/// </summary>
		/// <value>
		///  <c>true</c> if this instance is profile completed; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool CompletedProfile { get; init; }

		/// <summary>
		/// Gets or sets a value indicating whether [soft delete].
		/// </summary>
		/// <value>
		///  <c>true</c> if [soft delete]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool SoftDelete { get; init; }

		/// <summary>
		/// Gets a value indicating whether [locked user].
		/// </summary>
		/// <value>
		///   <c>true</c> if [locked user]; otherwise, <c>false</c>.
		/// </value>
		[DataMember]
		public bool LockedUser { get; init; }

		/// <summary>
		/// Gets the created on.
		/// </summary>
		/// <value>
		/// The created on.
		/// </value>
		[DataMember]
		public DateTime CreatedOn { get; init; }

		/// <summary>
		/// Gets the updated on.
		/// </summary>
		/// <value>
		/// The updated on.
		/// </value>
		[DataMember]
		public DateTime UpdatedOn { get; init; }

		/// <summary>
		/// Initializes a new instance of the <see cref="UserViewModel"/> class.
		/// </summary>
		public UserViewModel() { }
	}
}
