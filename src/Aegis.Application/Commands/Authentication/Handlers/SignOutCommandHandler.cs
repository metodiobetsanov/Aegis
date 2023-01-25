namespace Aegis.Application.Commands.Authentication.Handlers
{
	using System;

	using Aegis.Application.Commands.Authentication;
	using Aegis.Application.Constants;
	using Aegis.Application.Contracts;
	using Aegis.Application.Contracts.CQRS;
	using Aegis.Application.Exceptions;
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
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommandHandler&lt;Aegis.Application.Commands.Authentication.SignOutCommand, Aegis.Models.Authentication.SignOutCommandResult&gt;" />
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
		/// The sign in manager.
		/// </summary>
		private readonly SignInManager<AegisUser> _signInManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="SignOutCommandHandler"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="httpContext">The HTTP context.</param>
		/// <param name="interaction">The interaction.</param>
		/// <param name="events">The events.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public SignOutCommandHandler(
			ILogger<SignOutCommandHandler> logger,
			IHttpContextAccessor httpContext,
			IIdentityServerInteractionService interaction,
			IEventService events,
			SignInManager<AegisUser> signInManager)
		{
			_logger = logger;
			_httpContext = httpContext;
			_interaction = interaction;
			_events = events;
			_signInManager = signInManager;
		}

		/// <summary>
		/// Handles the specified query.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		///   <see cref="SignOutCommandResult" />
		/// </returns>
		/// <exception cref="Aegis.Application.Exceptions.IdentityProviderException"></exception>
		public async Task<SignOutCommandResult> Handle(SignOutCommand command, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignOutCommand));
			SignOutCommandResult signOutCommandResult = SignOutCommandResult.Failed();

			try
			{
				_logger.LogDebug("SignOutCommandHandler: check logout id.");
				string logoutId = command.LogoutId ?? await _interaction.CreateLogoutContextAsync();

				_logger.LogDebug("SignOutCommandHandler: sing out.");
				await _signInManager.SignOutAsync();
				await _signInManager.ForgetTwoFactorClientAsync();
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

					if (logoutRequest is null)
					{
						signOutCommandResult = SignOutCommandResult.Succeeded("~/");
					}
					else
					{
						signOutCommandResult = SignOutCommandResult.Succeeded(logoutRequest.PostLogoutRedirectUri);
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
