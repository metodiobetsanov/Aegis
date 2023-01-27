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
	using System.Linq;
	using System.Security.Cryptography;
	using System.Text;

	using Aegis.Core.Helpers;
	using Aegis.Models.Settings;

	using Microsoft.AspNetCore.Identity;

	/// <summary>
	/// Lookup Protector
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Identity.ILookupProtector" />
	public sealed class AegisLookupProtector : ILookupProtector
	{
		/// <summary>
		/// The settings
		/// </summary>
		private readonly IdentityProviderSettings _settings;

		/// <summary>
		/// The key ring
		/// </summary>
		private readonly ILookupProtectorKeyRing _keyRing;

		/// <summary>
		/// Initializes a new instance of the <see cref="AegisLookupProtector"/> class.
		/// </summary>
		/// <param name="keyRing">The keyring.</param>
		public AegisLookupProtector(IdentityProviderSettings settings, ILookupProtectorKeyRing keyRing)
		{
			_settings = settings;
			_keyRing = keyRing;
		}

		/// <summary>
		/// Protect the data using the specified key.
		/// </summary>
		/// <param name="keyId">The key to use.</param>
		/// <param name="data">The data to protect.</param>
		/// <returns>
		/// The protected data.
		/// </returns>
		public string? Protect(string keyId, string? data)
		{
			string? result = null;

			if (!string.IsNullOrWhiteSpace(data))
			{
				byte[] key = Convert.FromBase64String(_keyRing[keyId]);

				using (Aes aes = Aes.Create())
				using (HMACSHA512 sha512 = new HMACSHA512())
				{
					aes.KeySize = 256;
					aes.Key = IdentityProviderHelper.GetDerivation(_settings.LookupProtectorEncryptionDerivationPassword, key, aes.BlockSize / 8);
					aes.IV = IdentityProviderHelper.GetDerivation(data, key, aes.BlockSize / 8);
					sha512.Key = IdentityProviderHelper.GetDerivation(_settings.LookupProtectorSigningDerivationPassword, key, aes.BlockSize / 8);

					byte[] plainTextData = Encoding.UTF8.GetBytes(data);
					byte[] cipherTextAndIV;

					using (MemoryStream ms = new MemoryStream())
					using (CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(plainTextData);
						cs.FlushFinalBlock();
						cipherTextAndIV = IdentityProviderHelper.CombineArrays(aes.IV, ms.ToArray());
					}

					byte[] signature = sha512.ComputeHash(cipherTextAndIV);
					byte[] signedData = IdentityProviderHelper.CombineArrays(signature, cipherTextAndIV);

					result = Convert.ToBase64String(signedData);
				}
			}

			return result;
		}

		/// <summary>
		/// Unprotect the data using the specified key.
		/// </summary>
		/// <param name="keyId">The key to use.</param>
		/// <param name="data">The data to unprotect.</param>
		/// <returns>
		/// The original data.
		/// </returns>
		public string? Unprotect(string keyId, string? data)
		{
			string? result = null;

			if (!string.IsNullOrWhiteSpace(data))
			{
				byte[] key = Convert.FromBase64String(_keyRing[keyId]);

				using (Aes aes = Aes.Create())
				using (HMACSHA512 sha512 = new HMACSHA512())
				{
					aes.KeySize = 256;
					sha512.Key = IdentityProviderHelper.GetDerivation(_settings.LookupProtectorSigningDerivationPassword, key, aes.BlockSize / 8);

					byte[] signedData = Convert.FromBase64String(data);

					int signatureLenght = sha512.HashSize / 8;
					int dataLenght = signedData.Length - signatureLenght;

					byte[] signature = IdentityProviderHelper.SubstringArray(signedData, 0, signatureLenght);
					byte[] cipherTextAndIV = IdentityProviderHelper.SubstringArray(signedData, signatureLenght, dataLenght);
					byte[] computedSignature = sha512.ComputeHash(cipherTextAndIV);

					if (!signature.SequenceEqual(computedSignature))
					{
						throw new CryptographicException("Invalid Signature.");
					}

					int ivLength = aes.BlockSize / 8;
					int cipherTextLenght = cipherTextAndIV.Length - ivLength;
					byte[] initializationVector = IdentityProviderHelper.SubstringArray(cipherTextAndIV, 0, ivLength);
					byte[] cipherText = IdentityProviderHelper.SubstringArray(cipherTextAndIV, ivLength, cipherTextLenght);

					aes.Key = IdentityProviderHelper.GetDerivation(_settings.LookupProtectorEncryptionDerivationPassword, key, aes.BlockSize / 8);
					aes.IV = initializationVector;

					using (MemoryStream ms = new MemoryStream())
					using (CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherText);
						cs.FlushFinalBlock();
						result = Encoding.UTF8.GetString(ms.ToArray());
					}
				}
			}

			return result;
		}
	}
}
