namespace Aegis.Persistence.Entities.IdentityProvider
{
	using Microsoft.AspNetCore.Identity;

	/// <summary>
	/// AegisUserToken
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Identity.IdentityUserToken&lt;System.Guid&gt;" />
	/// <seealso cref="Aegis.Persistence.Entities.IEntity" />
	public class AegisUserToken : IdentityUserToken<Guid>, IEntity
	{
		/// <summary>
		/// A random value that should change whenever an entity is persisted to the store
		/// </summary>
		/// <value>
		/// The concurrency stamp.
		/// </value>
		public string? ConcurrencyStamp { get; set; }

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
		/// Initializes a new instance of the <see cref="AegisUserToken"/> class.
		/// </summary>
		public AegisUserToken()
		{
			this.ConcurrencyStamp = Guid.NewGuid().ToString();
			this.CreatedOn = DateTime.UtcNow;
		}
	}
}
