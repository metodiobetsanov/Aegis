#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Areas.Admin.Controllers
{
	using Aegis.Core.Events.Events;
	using Aegis.Core.Queries.IdentityProvider.Users;
	using Aegis.Models.Enums.Events;
	using Aegis.Models.Shared;
	using Aegis.Models.Users;

	using Duende.IdentityServer.Extensions;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	/// <summary>
	/// Users Controller
	/// </summary>
	/// <seealso cref="Controller" />
	[Authorize]
	public partial class UsersController : Controller
	{
		/// <summary>
		/// Gets all Users.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public async Task<JsonResult> AjaxGetUsers()
		{
			QueryResult<IList<UserViewModel>>? result = await _mediator.Send(new GetUsersQuery());
			return this.Json(result);
		}

		/// <summary>
		/// Resets the 2FA.
		/// </summary>
		/// <returns></returns>
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AjaxReset2FA()
		{
			await _mediator.Publish(new PushNotificationEvent(
				PushNotificationEventType.Info,
				PushNotificationEventAuditoryType.User,
				this.User.Identity.Name,
				"Hello"));

			await _mediator.Publish(new PushNotificationEvent(
	PushNotificationEventType.Success,
	PushNotificationEventAuditoryType.All,
	this.User.Identity.GetSubjectId(),
	"Hello"));

			await _mediator.Publish(new PushNotificationEvent(
	PushNotificationEventType.Warning,
	PushNotificationEventAuditoryType.All,
	this.User.Identity.GetSubjectId(),
	"Hello"));

			await _mediator.Publish(new PushNotificationEvent(
	PushNotificationEventType.Error,
	PushNotificationEventAuditoryType.All,
	this.User.Identity.GetSubjectId(),
	"Hello"));
			return this.Ok();
		}
	}
}
