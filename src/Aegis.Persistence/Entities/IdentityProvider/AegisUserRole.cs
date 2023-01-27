#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.Persistence.Entities.IdentityProvider
{
	using Microsoft.AspNetCore.Identity;

	/// <summary>
	/// AegisUserRole
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Identity.IdentityUserRole&lt;System.Guid&gt;" />
	/// <seealso cref="Aegis.Persistence.Entities.IEntity" />
	public class AegisUserRole : IdentityUserRole<Guid>, IEntity
	{
		/// <summary>
		/// Gets or sets the primary key of the user that is linked to a role.
		/// </summary>
		public override Guid UserId { get => base.UserId; set => base.UserId = value; }

		/// <summary>
		/// Gets or sets the user.
		/// </summary>
		/// <value>
		/// The user.
		/// </value>
		public virtual AegisUser User { get; set; }

		/// <summary>
		/// Gets or sets the primary key of the role that is linked to the user.
		/// </summary>
		public override Guid RoleId { get => base.RoleId; set => base.RoleId = value; }

		/// <summary>
		/// Gets or sets the role.
		/// </summary>
		/// <value>
		/// The role.
		/// </value>
		public virtual AegisRole Role { get; set; }

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
		/// Initializes a new instance of the <see cref="AegisUserRole"/> class.
		/// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
		public AegisUserRole()
		{
			this.ConcurrencyStamp = Guid.NewGuid().ToString();
			this.CreatedOn = DateTime.UtcNow;
		}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	}
}
