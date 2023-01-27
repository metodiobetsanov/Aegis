namespace Aegis.Application.Helpers
{
	using Microsoft.AspNetCore.DataProtection;
	using Microsoft.AspNetCore.Http;

	using Newtonsoft.Json;

	/// <summary>
	/// Protector Helpers
	/// </summary>
	public static class ProtectorHelpers
	{
		/// <summary>
		/// The query string protector
		/// </summary>
		public const string QueryStringProtector = "QueryStringProtector";

		/// <summary>
		/// The protected query string name
		/// </summary>
		public const string QueryStringНаме = "pqs";

		/// <summary>
		/// Protects the query string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dataProtector">The data protector.</param>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public static QueryString ProtectQueryString(IDataProtector dataProtector, object data)
		{
			string palinText = JsonConvert.SerializeObject(data);
			string encString = dataProtector.Protect(palinText);

			QueryString res = QueryString.Create(new List<KeyValuePair<string, string?>>
			{
				new KeyValuePair<string, string?>(QueryStringНаме, encString)
			});

			return res;
		}

		/// <summary>
		/// Protects the query string.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="dataProtector">The data protector.</param>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public static TResult? UnProtectQueryString<TResult>(IDataProtector dataProtector, string encString) where TResult : class
		{
			string palinText = dataProtector.Unprotect(encString);
			return JsonConvert.DeserializeObject<TResult>(palinText);
		}
	}
}
