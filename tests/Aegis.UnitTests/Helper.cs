namespace Aegis.UnitTests
{
	using System;
	using System.Text;

	public static class Helper
	{
		/// <summary>
		/// Converts to base64.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public static string ToBase64(this string data)
			=> Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
	}
}
