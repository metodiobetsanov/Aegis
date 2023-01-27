#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Services.DataProtection
{
	using System;

	using Aegis.Core.Constants;

	using Microsoft.AspNetCore.Identity;

	/// <summary>
	/// Aegis Personal Data Protector
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Identity.IPersonalDataProtector" />
	public class AegisPersonalDataProtector : IPersonalDataProtector
	{
		/// <summary>
		/// The key ring
		/// </summary>
		private readonly ILookupProtectorKeyRing _keyRing;

		/// <summary>
		/// The protector
		/// </summary>
		private readonly ILookupProtector _protector;

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisPersonalDataProtector"/> class.
		/// </summary>
		/// <param name="keyRing">The key ring.</param>
		/// <param name="protector">The protector.</param>
		public AegisPersonalDataProtector(ILookupProtectorKeyRing keyRing, ILookupProtector protector)
		{
			_keyRing = keyRing;
			_protector = protector;
		}

		/// <summary>
		/// Protect the data.
		/// </summary>
		/// <param name="data">The data to protect.</param>
		/// <returns>
		/// The protected data.
		/// </returns>
		public string? Protect(string? data)
		{
			string? result = null;

			if (!string.IsNullOrWhiteSpace(data))
			{
				string key = _keyRing.CurrentKeyId;
				string? protectedData = _protector.Protect(key, data);

				result = string.Format(IdentityProviderConstants.PersonalDataKeyDataStringFormat, key, protectedData);
			}

			return result;
		}

		/// <summary>
		/// Unprotect the data.
		/// </summary>
		/// <param name="data"></param>
		/// <returns>
		/// The unprotected data.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">Malformed data.</exception>
		public string? Unprotect(string? data)
		{
			string? result = null;

			if (!string.IsNullOrWhiteSpace(data))
			{
				string[] keyDataPair = data.Split(new char[] { '.' });

				if (keyDataPair.Length != 2)
				{
					throw new InvalidOperationException("Malformed data.");
				}

				result = _protector.Unprotect(keyDataPair[0], keyDataPair[1]);
			}

			return result;
		}
	}
}
