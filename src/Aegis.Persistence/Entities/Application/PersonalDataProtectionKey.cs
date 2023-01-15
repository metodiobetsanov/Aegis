namespace Aegis.Persistence.Entities.Application
{
	using System;

	using Aegis.Persistence.Attributes;

	/// <summary>
	/// Personal Data Protection Key
	/// </summary>
	/// <seealso cref="Aegis.Persistence.Entities.IEntity" />
	public class PersonalDataProtectionKey : IEntity
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>
		/// The identifier.
		/// </value>
		public required Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the key.
		/// </summary>
		/// <value>
		/// The key.
		/// </value>
		[SecureColumn]
		public required string Key { get; set; }

		/// <summary>
		/// Gets or sets the key hash.
		/// </summary>
		/// <value>
		/// The key hash.
		/// </value>
		[SecureColumn]
		public required string KeyHash { get; set; }
		/// <summary>
		/// Gets or sets the expires on.
		/// </summary>
		/// <value>
		/// The expires on.
		/// </value>
		public required DateTime ExpiresOn { get; set; }

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
		/// Initializes a new instance of the <see cref="PersonalDataProtectionKey"/> class.
		/// </summary>
		public PersonalDataProtectionKey()
		{
			this.ConcurrencyStamp = Guid.NewGuid().ToString();
			this.CreatedOn = DateTime.UtcNow;
		}
	}
}
