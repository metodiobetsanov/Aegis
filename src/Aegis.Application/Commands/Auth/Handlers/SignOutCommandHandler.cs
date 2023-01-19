namespace Aegis.Application.Commands.Auth.Handlers
{
	using Aegis.Application.Constants;
	using Aegis.Application.Contracts.CQRS;
	using Aegis.Application.Exceptions;
	using Aegis.Models.Shared;
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
	/// <seealso cref="ICommandHandler&lt;Aegis.Application.Commands.Auth.SignOutCommand, AuthenticationResult&gt;" />
	public sealed class SignOutCommandHandler : ICommandHandler<SignOutCommand, AuthenticationResult>
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
		/// <param name="query">The query.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		/// <exception cref="Exceptions.InvalidValueException"></exception>
		public async Task<AuthenticationResult> Handle(SignOutCommand command, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignOutCommand));
			AuthenticationResult authenticationResult = new AuthenticationResult(false);

			try
			{
				_logger.LogDebug("SignOutCommandHandler: check logout id.");
				string logoutId = command.LogoutId ?? await _interaction.CreateLogoutContextAsync();

				_logger.LogDebug("SignOutCommandHandler: sing out.");
				await _signInManager.SignOutAsync();
				await _events.RaiseAsync(new UserLogoutSuccessEvent(
					_httpContext.HttpContext!.User.GetSubjectId(),
					_httpContext.HttpContext!.User.GetDisplayName()));

				if (string.IsNullOrEmpty(logoutId))
				{
					authenticationResult = new AuthenticationResult("~/");
				}
				else
				{
					_logger.LogDebug("SignOutCommandHandler: get logout context.");
					LogoutRequest logoutRequest = await _interaction.GetLogoutContextAsync(logoutId);

					if (logoutRequest is null)
					{
						authenticationResult = new AuthenticationResult("~/");
					}
					else
					{
						authenticationResult = new AuthenticationResult(logoutRequest.PostLogoutRedirectUri);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SignOutCommandHandler Error: {Message}", ex.Message);
				throw new AuthenticationException(IdentityProviderConstants.SomethingWentWrongWithSignOut, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(SignOutCommand));
			return authenticationResult;
		}
	}
}
