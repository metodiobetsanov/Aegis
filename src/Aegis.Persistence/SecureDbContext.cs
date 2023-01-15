namespace Aegis.Persistence
{
	using System.Reflection.Emit;

	using Aegis.Persistence.Attributes;
	using Aegis.Persistence.Converters;
	using Aegis.Persistence.Entities.Application;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.DataProtection;
	using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Infrastructure;
	using Microsoft.EntityFrameworkCore.Metadata;

	/// <summary>
	/// Aegis Secure Db Context
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.IDataProtectionKeyContext" />
	public class SecureDbContext : DbContext, IDataProtectionKeyContext
	{
		/// <summary>
		/// A collection of <see cref="T:Microsoft.AspNetCore.DataProtection.EntityFrameworkCore.DataProtectionKey" />
		/// </summary>
		public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = default!;

		/// <summary>
		/// Gets the personal data protection keys.
		/// </summary>
		/// <value>
		/// The personal data protection keys.
		/// </value>
		public DbSet<PersonalDataProtectionKey> PersonalDataProtectionKeys { get; set; } = default!;

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureDbContext"/> class.
		/// </summary>
		/// <param name="options">The options to be used by a <see cref="T:Microsoft.EntityFrameworkCore.DbContext" />.</param>
		public SecureDbContext(DbContextOptions<SecureDbContext> options) : base(options)
		{

		}

		/// <summary>
		/// Override this method to further configure the model that was discovered by convention from the entity types
		/// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
		/// and re-used for subsequent instances of your derived context.
		/// </summary>
		/// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
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
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Set schema
			modelBuilder.HasDefaultSchema("aegis-sc");

			SecureColumnConverter encryptionConverter = new SecureColumnConverter(this.GetService<IDataProtectionProvider>().CreateProtector(nameof(SecureDbContext)));

			foreach (IMutableEntityType entityType in modelBuilder.Model.GetEntityTypes())
			{
				foreach (IMutableProperty property in entityType.GetProperties())
				{
					if (property.ClrType == typeof(string) && !(property.Name == "Discriminator" || property.PropertyInfo == null))
					{
						object[] attributes = property.PropertyInfo.GetCustomAttributes(typeof(SecureColumnAttribute), false);
						if (attributes.Any())
						{
							property.SetValueConverter(encryptionConverter);
						}
					}
				}
			}

			// PersonalDataProtectionKey table
			modelBuilder.Entity<PersonalDataProtectionKey>(b =>
			{
				// Maps to the PersonalDataProtectionKeys table
				b.ToTable("PersonalDataProtectionKeys");

				// A concurrency token for use with the optimistic concurrency checking
				b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
			});
		}
	}
}
