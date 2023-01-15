namespace Aegis.Persistence
{
	using System;
	using System.Reflection.Emit;
	using System.Threading;
	using System.Threading.Tasks;

	using Aegis.Persistence.Constants;
	using Aegis.Persistence.Entities;
	using Aegis.Persistence.Entities.IdentityProvider;
	using Aegis.Persistence.Exceptions;

	using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.ChangeTracking;

	/// <summary>
	/// Aegis Identity Db Context
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext&lt;Aegis.Persistence.Entities.IdentityProvider.AegisUser, Aegis.Persistence.Entities.IdentityProvider.AegisRole, System.Guid, Aegis.Persistence.Entities.IdentityProvider.AegisUserClaim, Aegis.Persistence.Entities.IdentityProvider.AegisUserRole, Aegis.Persistence.Entities.IdentityProvider.AegisUserLogin, Aegis.Persistence.Entities.IdentityProvider.AegisRoleClaim, Aegis.Persistence.Entities.IdentityProvider.AegisUserToken&gt;" />
	public class AegisIdentityDbContext : IdentityDbContext<AegisUser, AegisRole, Guid, AegisUserClaim, AegisUserRole, AegisUserLogin, AegisRoleClaim, AegisUserToken>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AegisIdentityDbContext"/> class.
		/// </summary>
		/// <param name="options">The options to be used by a <see cref="T:Microsoft.EntityFrameworkCore.DbContext" />.</param>
		public AegisIdentityDbContext(DbContextOptions<AegisIdentityDbContext> options) : base(options)
		{

		}

		/// <summary>
		/// Override this method to further configure the model that was discovered by convention from the entity types
		/// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
		/// and re-used for subsequent instances of your derived context.
		/// </summary>
		/// <param name="builder">The builder being used to construct the model for this context. Databases (and other extensions) typically
		/// define extension methods on this object that allow you to configure aspects of the model that are specific
		/// to a given database.</param>
		/// <remarks>
		/// <para>
		/// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
		/// then this method will not be run.
		/// </para>
		/// <para>
		/// See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> for more information.
		/// </para>
		/// </remarks>
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			// Set schema
			builder.HasDefaultSchema("aegis_idp");

			// AegisUser table
			builder.Entity<AegisUser>(b =>
			{
				// Maps to the AspNetUsers table
				b.ToTable("AegisUser");

				// A concurrency token for use with the optimistic concurrency checking
				b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

				// Each User can have many entries in the UserRole join table
				b.HasMany(e => e.UserRoles)
					.WithOne(e => e.User)
					.HasForeignKey(ur => ur.UserId)
					.IsRequired();
			});

			// AegisUserClaims table
			builder.Entity<AegisUserClaim>(b =>
			{
				// Maps to the AegisUserClaims table
				b.ToTable("AegisUserClaims");

				// A concurrency token for use with the optimistic concurrency checking
				b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
			});

			// AegisUserLogin table
			builder.Entity<AegisUserLogin>(b =>
			{
				// Maps to the AegisUserLogin table
				b.ToTable("AegisUserLogin");

				// A concurrency token for use with the optimistic concurrency checking
				b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
			});

			// AegisUserToken table
			builder.Entity<AegisUserToken>(b =>
			{
				// Maps to the AegisUserToken table
				b.ToTable("AegisUserToken");

				// A concurrency token for use with the optimistic concurrency checking
				b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
			});

			// AegisRole table
			builder.Entity<AegisRole>(b =>
			{
				// Maps to the AegisRole table
				b.ToTable("AegisRole");

				// A concurrency token for use with the optimistic concurrency checking
				b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

				// Each Role can have many entries in the UserRole join table
				b.HasMany(e => e.UserRoles)
					.WithOne(e => e.Role)
					.HasForeignKey(ur => ur.RoleId)
					.IsRequired();
			});

			// AegisRoleClaim table
			builder.Entity<AegisRoleClaim>(b =>
			{
				// Maps to the AspNetRoleClaims table
				b.ToTable("AegisRoleClaim");

				// A concurrency token for use with the optimistic concurrency checking
				b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
			});

			// AegisUserRole table
			builder.Entity<AegisUserRole>(b =>
			{
				// Maps to the AegisUserRole table
				b.ToTable("AegisUserRole");

				// A concurrency token for use with the optimistic concurrency checking
				b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
			});
		}

		/// <summary>
		/// Saves all changes made in this context to the database.
		/// </summary>
		/// <returns>
		/// The number of state entries written to the database.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" />
		/// to discover any changes to entity instances before saving to the underlying database. This can be disabled via
		/// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
		/// </para>
		/// <para>
		/// Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This
		/// includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
		/// Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
		/// in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information
		/// and examples.
		/// </para>
		/// <para>
		/// See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for more information and examples.
		/// </para>
		/// </remarks>
		public override int SaveChanges()
		{
			this.SetTimeStamps();
			return base.SaveChanges();
		}

		/// <summary>
		/// Saves all changes made in this context to the database.
		/// </summary>
		/// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" />
		/// is called after the changes have been sent successfully to the database.</param>
		/// <returns>
		/// The number of state entries written to the database.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" />
		/// to discover any changes to entity instances before saving to the underlying database. This can be disabled via
		/// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
		/// </para>
		/// <para>
		/// Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This
		/// includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
		/// Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
		/// in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more information
		/// and examples.
		/// </para>
		/// <para>
		/// See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for more information and examples.
		/// </para>
		/// </remarks>
		public override int SaveChanges(bool acceptAllChangesOnSuccess)
		{
			this.SetTimeStamps();
			return base.SaveChanges(acceptAllChangesOnSuccess);
		}

		/// <summary>
		/// Saves all changes made in this context to the database.
		/// </summary>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
		/// <returns>
		/// A task that represents the asynchronous save operation. The task result contains the
		/// number of state entries written to the database.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" />
		/// to discover any changes to entity instances before saving to the underlying database. This can be disabled via
		/// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
		/// </para>
		/// <para>
		/// Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This
		/// includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
		/// Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
		/// in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more
		/// information and examples.
		/// </para>
		/// <para>
		/// See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for more information and examples.
		/// </para>
		/// </remarks>
		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			this.SetTimeStamps();
			return base.SaveChangesAsync(cancellationToken);
		}

		/// <summary>
		/// Saves all changes made in this context to the database.
		/// </summary>
		/// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" /> is called after
		/// the changes have been sent successfully to the database.</param>
		/// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
		/// <returns>
		/// A task that represents the asynchronous save operation. The task result contains the
		/// number of state entries written to the database.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" />
		/// to discover any changes to entity instances before saving to the underlying database. This can be disabled via
		/// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
		/// </para>
		/// <para>
		/// Entity Framework Core does not support multiple parallel operations being run on the same DbContext instance. This
		/// includes both parallel execution of async queries and any explicit concurrent use from multiple threads.
		/// Therefore, always await async calls immediately, or use separate DbContext instances for operations that execute
		/// in parallel. See <see href="https://aka.ms/efcore-docs-threading">Avoiding DbContext threading issues</see> for more
		/// information and examples.
		/// </para>
		/// <para>
		/// See <see href="https://aka.ms/efcore-docs-saving-data">Saving data in EF Core</see> for more information and examples.
		/// </para>
		/// </remarks>
		public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
		{
			this.SetTimeStamps();
			return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}

		/// <summary>
		/// Sets the updated value before save.
		/// </summary>
		private void SetTimeStamps()
		{
			IEnumerable<EntityEntry> entries = this.ChangeTracker.Entries();
			DateTime utcNow = DateTime.UtcNow;

			foreach (EntityEntry entry in entries)
			{
				// for entities that inherit from IAegisEntity,
				// set UpdatedOn appropriately
				if (entry.Entity is IEntity aegisEntity)
				{
					switch (entry.State)
					{
						case EntityState.Modified:
							// set the updated date to "now"
							aegisEntity.UpdatedOn = utcNow;

							// mark property as "don't touch"
							// we don't want to update on a Modify operation
							entry.Property("CreatedOn").IsModified = false;
							break;
					}
				}
				else if (entry.Entity is not DataProtectionKey)
				{
					throw new PersistenceException(
						PersistenceConstants.SomethingWentWrong,
						$"The entity {entry.Entity.GetType().Name} does not inherit 'IAegisEntity'!");
				}
			}
		}
	}
}
