namespace Aegis.Persistence.Entities.IdentityProvider
{
	using Microsoft.AspNetCore.Identity;

	/// <summary>
	/// Aegis Role
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Identity.IdentityRole&lt;System.Guid&gt;" />
	/// <seealso cref="Aegis.Persistence.Entities.IAegisEntity" />
	public class AegisRole : IdentityRole<Guid>, IAegisEntity
	{
		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>
		/// The description.
		/// </value>
		public string? Description { get; set; }

		/// <summary>
		/// Gets or sets the claims.
		/// </summary>
		/// <value>
		/// The claims.
		/// </value>
		public virtual HashSet<AegisRoleClaim> Claims { get; set; }

		/// <summary>
		/// Gets or sets the user roles.
		/// </summary>
		/// <value>
		/// The user roles.
		/// </value>
		public virtual HashSet<AegisUserRole> UserRoles { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [system role].
		/// </summary>
		/// <value>
		///   <c>true</c> if [system role]; otherwise, <c>false</c>.
		/// </value>
		public bool SystemRole { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [protected role].
		/// </summary>
		/// <value>
		///   <c>true</c> if [protected role]; otherwise, <c>false</c>.
		/// </value>
		public bool ProtectedRole { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [assign by default].
		/// </summary>
		/// <value>
		///   <c>true</c> if [assign by default]; otherwise, <c>false</c>.
		/// </value>
		public bool AssignByDefault { get; set; }

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
		/// Initializes a new instance of the <see cref="AegisRole"/> class.
		/// </summary>
		public AegisRole()
			: base()
		{
			this.Claims = new HashSet<AegisRoleClaim>();
			this.UserRoles = new HashSet<AegisUserRole>();
			this.ConcurrencyStamp = Guid.NewGuid().ToString();
			this.CreatedOn = DateTime.UtcNow;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisRole"/> class.
		/// </summary>
		/// <param name="roleName">The role name.</param>
		public AegisRole(string roleName)
			: this(roleName, nameof(Description), false, false, false)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisRole"/> class.
		/// </summary>
		/// <param name="roleName">Name of the role.</param>
		/// <param name="description">The description.</param>
		public AegisRole(string roleName, string description)
			: this(roleName, description, false, false, false)
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisRole"/> class.
		/// </summary>
		/// <param name="roleName">Name of the role.</param>
		/// <param name="description">The description.</param>
		/// <param name="protectedRole">if set to <c>true</c> [protected role].</param>
		/// <param name="systemRole">if set to <c>true</c> [system role].</param>
		/// <param name="assignByDefault">if set to <c>true</c> [assign by default].</param>
		public AegisRole(
			string roleName,
			string description,
			bool protectedRole,
			bool systemRole,
			bool assignByDefault)
			: base(roleName)
		{
			this.Description = description;
			this.Claims = new HashSet<AegisRoleClaim>();
			this.UserRoles = new HashSet<AegisUserRole>();
			this.ProtectedRole = protectedRole;
			this.SystemRole = systemRole;
			this.AssignByDefault = assignByDefault;
			this.ConcurrencyStamp = Guid.NewGuid().ToString();
			this.CreatedOn = DateTime.UtcNow;
		}
	}
}
