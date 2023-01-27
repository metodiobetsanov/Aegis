#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
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
		/// Gets the audit logs.
		/// </summary>
		/// <value>
		/// The audit logs.
		/// </value>
		IRepository<AuditLog> AuditLogs { get; }

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
