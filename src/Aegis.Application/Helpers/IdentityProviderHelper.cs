namespace Aegis.Application.Helpers
{
	using System.Security.Cryptography;
	using System.Text;

	using Aegis.Application.Constants;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Cryptography.KeyDerivation;

	using static System.Runtime.InteropServices.JavaScript.JSType;
	using static IdentityModel.OidcConstants;

	/// <summary>
	/// Identity Provider Helper
	/// </summary>
	public static class IdentityProviderHelper
	{
		/// <summary>
		/// Gets the full name.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns></returns>
		public static string GetFullName(this AegisUser user)
		{
			return $"{user.FirstName} {user.LastName}";
		}

		/// <summary>
		/// Converts to base64 string.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public static string ToBase64String(this string data)
			=> Convert.ToBase64String(Encoding.UTF8.GetBytes(data));

		/// <summary>
		/// Converts from base64 string.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public static string FromBase64String(this string data)
			=> Encoding.UTF8.GetString(Convert.FromBase64String(data));

		/// <summary>
		/// Generates a random password.
		/// </summary>
		/// <param name="passwordLength">Length of the password.</param>
		/// <returns></returns>
		public static string GenerateRandomPassword()
		{
			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < IdentityProviderConstants.PasswordLength; i++)
			{
				byte[] byteArray = new byte[1];
				char c;

				do
				{
					RandomNumberGenerator.Fill(byteArray);
					c = (char)byteArray[0];

				} while (!IdentityProviderConstants.AllChar.Any(x => x == c));

				sb = sb.Append(c);
			}

			return sb.ToString();
		}

		/// <summary>
		/// Combines the byte arrays.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns></returns>
		public static TType[] CombineArrays<TType>(TType[] left, TType[] right)
		{
#pragma warning disable CA2018 // 'Buffer.BlockCopy' expects the number of bytes to be copied for the 'count' argument
			TType[] output = new TType[left.Length + right.Length];
			Buffer.BlockCopy(left, 0, output, 0, left.Length);
			Buffer.BlockCopy(right, 0, output, left.Length, right.Length);
#pragma warning restore CA2018 // 'Buffer.BlockCopy' expects the number of bytes to be copied for the 'count' argument
			return output;
		}

		/// <summary>
		/// Substrings the array.
		/// </summary>
		/// <typeparam name="TType">The type of the type.</typeparam>
		/// <param name="source">The source.</param>
		/// <param name="sourceOffset">The source offset.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public static TType[] SubstringArray<TType>(TType[] source, int sourceOffset, int count)
		{
#pragma warning disable CA2018 // 'Buffer.BlockCopy' expects the number of bytes to be copied for the 'count' argument
			TType[] output = new TType[count];
			Buffer.BlockCopy(source, sourceOffset, output, 0, count);
#pragma warning restore CA2018 // 'Buffer.BlockCopy' expects the number of bytes to be copied for the 'count' argument
			return output;
		}

		/// <summary>
		/// Gets the derivation.
		/// </summary>
		/// <param name="password">The password.</param>
		/// <param name="salt">The salt.</param>
		/// <param name="bytesRequested">The bytes requested.</param>
		/// <returns></returns>
		public static byte[] GetDerivation(string password, byte[] salt, int bytesRequested)
			=> KeyDerivation.Pbkdf2(password, salt, KeyDerivationPrf.HMACSHA512, IdentityProviderConstants.KeyDerivationIterationCount, bytesRequested);

	}
}
