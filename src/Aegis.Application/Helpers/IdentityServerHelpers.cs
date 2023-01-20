namespace Aegis.Application.Helpers
{
	using System.Diagnostics.CodeAnalysis;
	using System.Security.Authentication;

	using Aegis.Application.Exceptions;

	/// <summary>
	/// Identity Server Helpers
	/// </summary>
	public static class IdentityServerHelpers
	{
		/// <summary>
		/// Checks the is local URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		public static bool CheckIsLocalUrl([NotNullWhen(true)] string? url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return false;
			}

			// Allows "/" or "/foo" but not "//" or "/\".
			if (url[0] == '/')
			{
				// url is exactly "/"
				if (url.Length == 1)
				{
					return true;
				}

				// url doesn't start with "//" or "/\"
				if (url[1] != '/' && url[1] != '\\')
				{
					return !HasControlCharacter(url.AsSpan(1));
				}

				return false;
			}

			// Allows "~/" or "~/foo" but not "~//" or "~/\".
			if (url[0] == '~' && url.Length > 1 && url[1] == '/')
			{
				// url is exactly "~/"
				if (url.Length == 2)
				{
					return true;
				}

				// url doesn't start with "~//" or "~/\"
				if (url[2] != '/' && url[2] != '\\')
				{
					return !HasControlCharacter(url.AsSpan(2));
				}

				return false;
			}

			return false;

			static bool HasControlCharacter(ReadOnlySpan<char> readOnlySpan)
			{
				// URLs may not contain ASCII control characters.
				for (int i = 0; i < readOnlySpan.Length; i++)
				{
					if (char.IsControl(readOnlySpan[i]))
					{
						return true;
					}
				}

				return false;
			}
		}

		/// <summary>
		/// Gets the return URL.
		/// </summary>
		/// <param name="returnUrl">The return URL.</param>
		/// <returns></returns>
		/// <exception cref="Chimera.Application.Exceptions.AuthenticationException">Invalid return URL!</exception>
		public static string GetReturnUrl(string? returnUrl)
		{
			if (CheckIsLocalUrl(returnUrl))
			{
				return returnUrl;
			}
			else if (string.IsNullOrWhiteSpace(returnUrl))
			{
				return "~/";
			}
			else
			{
				throw new IdentityServerException("Invalid return URL!");
			}
		}
	}
}
