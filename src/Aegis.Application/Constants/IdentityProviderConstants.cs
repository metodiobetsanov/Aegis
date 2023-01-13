﻿namespace Aegis.Application.Constants
{
	/// <summary>
	/// Identity Provider Constants
	/// </summary>
	public static class IdentityProviderConstants
	{
		/// <summary>
		/// The Identity cookie
		/// </summary>
		public const string IdentityCookie = "AegisIdentityCookie";

		/// <summary>
		/// The root role
		/// </summary>
		public const string RootRole = "root";

		/// <summary>
		/// The operator role
		/// </summary>
		public const string OperatorRole = "operator";

		/// <summary>
		/// The user role
		/// </summary>
		public const string UserRole = "user";

		/// <summary>
		/// The root user name
		/// </summary>
		public const string RootUserName = "aegis@root.service";

		/// <summary>
		/// The password length
		/// </summary>
		public const int PasswordLength = 32;

		/// <summary>
		/// The capital letters
		/// </summary>
		public const string CapitalLetters = "QWERTYUIOPASDFGHJKLZXCVBNM";

		/// <summary>
		/// The small letters
		/// </summary>
		public const string SmallLetters = "qwertyuiopasdfghjklzxcvbnm";

		/// <summary>
		/// The digits
		/// </summary>
		public const string Digits = "0123456789";

		/// <summary>
		/// The special characters
		/// </summary>
		public const string SpecialCharacters = "!@#$%^&*()-_=+<,>.";

		/// <summary>
		/// All character
		/// </summary>
		public const string AllChar = CapitalLetters + SmallLetters + Digits + SpecialCharacters;

		/// <summary>
		/// The key derivation iteration count
		/// </summary>
		public const int KeyDerivationIterationCount = 10000;

		/// <summary>
		/// The personal data key data delimiter
		/// </summary>
		public const string PersonalDataKeyDataStringFormat = @"{0}.{1}";
	}
}