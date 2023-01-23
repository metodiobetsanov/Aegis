namespace Aegis.Application.Helpers
{
	using System.Collections.Generic;

	using Aegis.Models.Shared;

	using Microsoft.AspNetCore.Mvc.ModelBinding;

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
	}
}
