namespace Aegis.Persistence.Entities.IdentityProvider
{
	using Microsoft.AspNetCore.Identity;

	/// <summary>
	/// AegisUser
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Identity.IdentityUser&lt;System.Guid&gt;" />
	/// <seealso cref="Aegis.Persistence.Entities.IAegisEntity" />
	public class AegisUser : IdentityUser<Guid>, IAegisEntity
	{
		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>
		/// The first name.
		/// </value>
		[ProtectedPersonalData]
		public string? FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>
		/// The last name.
		/// </value>
		[ProtectedPersonalData]
		public string? LastName { get; set; }

		/// <summary>
		/// Gets or sets the profile picture URL.
		/// </summary>
		/// <value>
		/// The profile picture URL.
		/// </value>
		[ProtectedPersonalData]
		public string ProfilePictureURL { get; set; }

		/// <summary>
		/// Gets or sets the claims.
		/// </summary>
		/// <value>
		/// The claims.
		/// </value>
		public virtual HashSet<AegisUserClaim> Claims { get; set; }

		/// <summary>
		/// Gets or sets the user roles.
		/// </summary>
		/// <value>
		/// The user roles.
		/// </value>
		public virtual HashSet<AegisUserRole> UserRoles { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is system user.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is system user; otherwise, <c>false</c>.
		/// </value>
		public bool SystemUser { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is protected user.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is protected user; otherwise, <c>false</c>.
		/// </value>
		public bool ProtectedUser { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [active profile].
		/// </summary>
		/// <value>
		///   <c>true</c> if [active profile]; otherwise, <c>false</c>.
		/// </value>
		public bool ActiveProfile { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is profile completed.
		/// </summary>
		/// <value>
		///   <c>true</c> if this instance is profile completed; otherwise, <c>false</c>.
		/// </value>
		public bool CompletedProfile { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [soft delete].
		/// </summary>
		/// <value>
		///   <c>true</c> if [soft delete]; otherwise, <c>false</c>.
		/// </value>
		public bool SoftDelete { get; set; }

		/// <summary>
		/// Gets or sets the created on.
		/// </summary>
		/// <value>
		/// The created on.
		/// </value>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the updated on.
		/// </summary>
		/// <value>
		/// The updated on.
		/// </value>
		public DateTime? UpdatedOn { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisUser"/> class.
		/// </summary>
		public AegisUser()
			: base()
		{
			this.ProfilePictureURL = "https://www.odysseyhouse.com.au/wordpress/wp-content/uploads/2019/08/Profile-Photo-Place-Holder.png";
			this.Claims = new HashSet<AegisUserClaim>();
			this.UserRoles = new HashSet<AegisUserRole>();
			this.ConcurrencyStamp = Guid.NewGuid().ToString();
			this.CreatedOn = DateTime.UtcNow;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisUser"/> class.
		/// </summary>
		/// <param name="email">The email.</param>
		public AegisUser(string email)
			: this(email, true, false)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisUser"/> class.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <param name="activeProfile">if set to <c>true</c> [active profile].</param>
		/// <param name="completedProfile">if set to <c>true</c> [completed profile].</param>
		public AegisUser(string email, bool activeProfile, bool completedProfile)
			: this(email, false, false, activeProfile, completedProfile)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisUser"/> class.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <param name="systemUser">if set to <c>true</c> [system user].</param>
		/// <param name="protectedUser">if set to <c>true</c> [protected user].</param>
		/// <param name="activeProfile">if set to <c>true</c> [active profile].</param>
		/// <param name="completedProfile">if set to <c>true</c> [completed profile].</param>
		public AegisUser(string email, bool systemUser, bool protectedUser, bool activeProfile, bool completedProfile)
			: base(email)
		{
			this.Email = email;
			this.ProfilePictureURL = "https://www.odysseyhouse.com.au/wordpress/wp-content/uploads/2019/08/Profile-Photo-Place-Holder.png";
			this.Claims = new HashSet<AegisUserClaim>();
			this.UserRoles = new HashSet<AegisUserRole>();
			this.SystemUser = systemUser;
			this.ProtectedUser = protectedUser;
			this.ActiveProfile = activeProfile;
			this.CompletedProfile = completedProfile;
			this.ConcurrencyStamp = Guid.NewGuid().ToString();
			this.CreatedOn = DateTime.UtcNow;
		}
	}
}
