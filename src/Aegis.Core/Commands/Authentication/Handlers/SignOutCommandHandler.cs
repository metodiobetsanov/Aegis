#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Commands.Authentication.Handlers
{
	using System;

	using Aegis.Core.Commands.Authentication;
	using Aegis.Core.Constants;
	using Aegis.Core.Contracts;
	using Aegis.Core.Contracts.CQRS;
	using Aegis.Core.Exceptions;
	using Aegis.Core.Helpers;
	using Aegis.Models.Authentication;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Duende.IdentityServer.Events;
	using Duende.IdentityServer.Extensions;
	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// SignOut Command Handler
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.ICommandHandler&lt;Aegis.Core.Commands.Authentication.SignOutCommand, Aegis.Models.Authentication.SignOutCommandResult&gt;" />
	public sealed class SignOutCommandHandler : ICommandHandler<SignOutCommand, SignOutCommandResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<SignOutCommandHandler> _logger;

		/// <summary>
		/// The HTTP context
		/// </summary>
		private readonly IHttpContextAccessor _httpContext;

		/// <summary>
		/// The interaction.
		/// </summary>
		private readonly IIdentityServerInteractionService _interaction;

		/// <summary>
		/// The events.
		/// </summary>
		private readonly IEventService _events;

		/// <summary>
		/// The user manager
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// The sign in manager.
		/// </summary>
		private readonly SignInManager<AegisUser> _signInManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="SignOutCommandHandler" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="httpContext">The HTTP context.</param>
		/// <param name="interaction">The interaction.</param>
		/// <param name="events">The events.</param>
		/// <param name="userManager">The user manager.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public SignOutCommandHandler(
			ILogger<SignOutCommandHandler> logger,
			IHttpContextAccessor httpContext,
			IIdentityServerInteractionService interaction,
			IEventService events,
			UserManager<AegisUser> userManager,
			SignInManager<AegisUser> signInManager)
		{
			_logger = logger;
			_httpContext = httpContext;
			_interaction = interaction;
			_events = events;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		/// <summary>
		/// Handles the specified query.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		///  <see cref="SignOutCommandResult" />
		/// </returns>
		/// <exception cref="Aegis.Core.Exceptions.IdentityProviderException"></exception>
		public async Task<SignOutCommandResult> Handle(SignOutCommand command, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignOutCommand));
			SignOutCommandResult signOutCommandResult = SignOutCommandResult.Succeeded("~/");

			try
			{
				_logger.LogDebug("SignOutCommandHandler: check logout id.");
				string logoutId = command.LogoutId ?? await _interaction.CreateLogoutContextAsync();
				AegisUser? user = await _userManager.FindByIdAsync(_httpContext.HttpContext!.User.GetSubjectId());

				if (user is not null)
				{
					if (command.SignOutAllSessions)
					{
						IdentityResult securityStampResult = await _userManager.UpdateSecurityStampAsync(user);

						if (!securityStampResult.Succeeded)
						{
							signOutCommandResult = SignOutCommandResult.Failed();
							securityStampResult.AddToFailedResult(signOutCommandResult);
							return signOutCommandResult;
						}
					}

					if (command.ForgetClient)
					{
						await _signInManager.ForgetTwoFactorClientAsync();
					}

					_logger.LogDebug("SignOutCommandHandler: sing out.");
					await _signInManager.SignOutAsync();
					await _events.RaiseAsync(new UserLogoutSuccessEvent(
						_httpContext.HttpContext!.User.GetSubjectId(),
						_httpContext.HttpContext!.User.GetDisplayName()));

					if (string.IsNullOrEmpty(logoutId))
					{
						signOutCommandResult = SignOutCommandResult.Succeeded("~/");
					}
					else
					{
						_logger.LogDebug("SignOutCommandHandler: get logout context.");
						LogoutRequest logoutRequest = await _interaction.GetLogoutContextAsync(logoutId);

						signOutCommandResult = logoutRequest is null
							? SignOutCommandResult.Succeeded("~/")
							: SignOutCommandResult.Succeeded(logoutRequest.PostLogoutRedirectUri);
					}
				}
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "SignOutCommandHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrongWithSignOut, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(SignOutCommand));
			return signOutCommandResult;
		}
	}
}
