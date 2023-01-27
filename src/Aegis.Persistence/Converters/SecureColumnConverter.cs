#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.Persistence.Converters
{
	using Microsoft.AspNetCore.DataProtection;
	using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

	/// <summary>
	/// Secure Column Converter
	/// </summary>
	/// <seealso cref="Microsoft.EntityFrameworkCore.Storage.ValueConversion.ValueConverter&lt;System.String, System.String&gt;" />
	internal sealed class SecureColumnConverter : ValueConverter<string, string>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SecureColumnConverter"/> class.
		/// </summary>
		/// <param name="protector">The protector.</param>
		public SecureColumnConverter(IDataProtector protector)
			: base(x => protector.Protect(x), x => protector.Unprotect(x))
		{
		}
	}
}
