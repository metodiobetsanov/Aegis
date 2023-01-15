namespace Aegis.Persistence.Contracts
{
	using Aegis.Persistence.Entities.Application;

	/// <summary>
	/// Aegis Context
	/// </summary>
	public interface IAegisContext : IDisposable
	{
		/// <summary>
		/// Gets the personal data protection keys.
		/// </summary>
		/// <value>
		/// The personal data protection keys.
		/// </value>
		IRepository<PersonalDataProtectionKey> PersonalDataProtectionKeys { get; }

		/// <summary>
		/// Saves the changes.
		/// </summary>
		int SaveChanges();

		/// <summary>
		/// Saves the changes asynchronous.
		/// </summary>
		Task<int> SaveChangesAsync();
	}
}
