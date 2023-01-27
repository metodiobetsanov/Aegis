#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.Persistence
{
	using Aegis.Persistence.Contracts;
	using Aegis.Persistence.Entities.Application;
	using Aegis.Persistence.Repositories;

	/// <summary>
	/// Aegis Context
	/// </summary>
	/// <seealso cref="Aegis.Persistence.Contracts.IAegisContext" />
	public sealed class AegisContext : IAegisContext
	{
		/// <summary>
		/// The disposed
		/// </summary>
		private bool _disposed;

		/// <summary>
		/// The secure database context
		/// </summary>
		private readonly SecureDbContext _secureDbContext;

		/// <summary>
		/// The personal data protection keys
		/// </summary>
		private IRepository<PersonalDataProtectionKey>? _personalDataProtectionKeys;

		/// <summary>
		/// The personal data protection keys
		/// </summary>
		private IRepository<AuditLog>? _auditLogs;

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisContext"/> class.
		/// </summary>
		/// <param name="secureDbContext">The secure database context.</param>
		public AegisContext(SecureDbContext secureDbContext)
		{
			_secureDbContext = secureDbContext;
		}

		/// <summary>
		/// Gets the personal data protection keys.
		/// </summary>
		/// <value>
		/// The personal data protection keys.
		/// </value>
		public IRepository<PersonalDataProtectionKey> PersonalDataProtectionKeys
		{
			get
			{
				_personalDataProtectionKeys ??= new Repository<PersonalDataProtectionKey>(_secureDbContext);

				return _personalDataProtectionKeys;
			}
		}

		/// <summary>
		/// Gets the audit logs.
		/// </summary>
		/// <value>
		/// The audit logs.
		/// </value>
		public IRepository<AuditLog> AuditLogs
		{
			get
			{
				_auditLogs ??= new Repository<AuditLog>(_secureDbContext);

				return _auditLogs;
			}
		}

		/// <summary>
		/// Saves the changes.
		/// </summary>
		public int SaveChanges()
		{
			int changes = 0;

			if (_secureDbContext.ChangeTracker.HasChanges())
			{
				changes += _secureDbContext.SaveChanges();
			}

			return changes;
		}

		/// <summary>
		/// Saves the changes asynchronous.
		/// </summary>
		public async Task<int> SaveChangesAsync()
		{
			int changes = 0;

			if (_secureDbContext.ChangeTracker.HasChanges())
			{
				changes += await _secureDbContext.SaveChangesAsync();
			}

			return changes;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		private void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				_secureDbContext.Dispose();
			}

			_disposed = true;
		}
	}
}
