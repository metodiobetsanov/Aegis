#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Helpers
{
	using System.Collections.Generic;

	using Aegis.Models.Authentication;
	using Aegis.Models.Shared;

	using Duende.IdentityServer.Models;

	using Microsoft.AspNetCore.Identity;
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
		/// Adds to failed result.
		/// </summary>
		/// <param name="identityResult">The identity result.</param>
		/// <param name="failedResult">The failed result.</param>
		public static void AddToFailedResult(this IdentityResult identityResult, BaseResult failedResult)
		{
			if (!identityResult.Succeeded && !failedResult.Success)
			{
				foreach (IdentityError error in identityResult.Errors)
				{
					failedResult.Errors.Add(new KeyValuePair<string, string>(error.Code, error.Description));
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
