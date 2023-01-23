namespace Aegis.Application.Helpers
{
	using System.Collections.Generic;

	using Aegis.Models.Shared;

	using Duende.IdentityServer.Models;

	using Microsoft.AspNetCore.Mvc.ModelBinding;

	using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

	/// <summary>
	/// Application Helpers
	/// </summary>
	public static class ApplicationHelpers
	{
		/// <summary>
		/// Adds the state of to model.
		/// </summary>
		/// <param name="result">The result.</param>
		/// <param name="modelState">State of the model.</param>
		public static void AddToModelState(this BaseResult result, ModelStateDictionary modelState)
		{
			if (result.Errors.Count > 0)
			{
				foreach (KeyValuePair<string, string> error in result.Errors)
				{
					modelState.AddModelError(error.Key, error.Value);
				}
			}
		}

		/// <summary>
		/// Gets the return URL.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="returnUrl">The return URL.</param>
		/// <returns></returns>
		public static string GetReturnUrl(this AuthorizationRequest context, string returnUrl)
		{
			string url;

			if (context == null)
			{
				url = IdentityServerHelpers.GetReturnUrl(returnUrl);
			}
			else
			{
				url = returnUrl;
			}

			return url;
		}
	}
}
