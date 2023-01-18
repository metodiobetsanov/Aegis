namespace Aegis.Application.Commands.Auth.Handlers
{
	using Aegis.Application.Constants;
	using Aegis.Application.Contracts.CQRS;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Helpers;
	using Aegis.Models.Shared;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Duende.IdentityServer.Events;
	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// SignIn Command Handler
	/// </summary>
	/// <seealso cref="Chimera.Application.Contracts.CQRS.ICommandHandler&lt;Chimera.Application.Commands.Authentication.SignInCommand, Chimera.Models.Authentication.AuthenticationResult&gt;" />
	public sealed class SignInCommandHandler : ICommandHandler<SignInCommand, AuthenticationResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<SignInCommandHandler> _logger;

		/// <summary>
		/// The interaction.
		/// </summary>
		private readonly IIdentityServerInteractionService _interaction;

		/// <summary>
		/// The events.
		/// </summary>
		private readonly IEventService _events;

		/// <summary>
		/// The user manager.
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// The sign in manager.
		/// </summary>
		private readonly SignInManager<AegisUser> _signInManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInCommandHandler"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="interaction">The interaction.</param>
		/// <param name="events">The events.</param>
		/// <param name="userManager">The user manager.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public SignInCommandHandler(
			ILogger<SignInCommandHandler> logger,
			IIdentityServerInteractionService interaction,
			IEventService events,
			UserManager<AegisUser> userManager,
			SignInManager<AegisUser> signInManager)
		{
			_logger = logger;
			_interaction = interaction;
			_events = events;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		/// <summary>
		/// Handles the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		/// <exception cref="InvalidValueException"></exception>
		public async Task<AuthenticationResult> Handle(SignInCommand command, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignInCommand));
			AuthenticationResult authenticationResult = new AuthenticationResult(false);

			try
			{
				_logger.LogDebug("SignInCommandHandler: get authorization context and validate return URL.");
				string returnUrl;
				AuthorizationRequest? context = await _interaction.GetAuthorizationContextAsync(command.ReturnUrl);

				if (context == null)
				{
					returnUrl = IdentityServerHelpers.GetReturnUrl(command.ReturnUrl);
				}
				else
				{
					returnUrl = command.ReturnUrl!;
				}

				_logger.LogDebug("SignInCommandHandler: check if user exists.");
				AegisUser? user = await _userManager.FindByEmailAsync(command.Email!);

				if (user is null)
				{
					_logger.LogDebug("SignInCommandHandler: user does not exists.");
					await _events.RaiseAsync(new UserLoginFailureEvent(command.Email, "Invalid credentials", clientId: context?.Client.ClientId));
				}
				else
				{
					_logger.LogDebug("SignInCommandHandler:  user exists.");
					SignInResult result = await _signInManager.PasswordSignInAsync(user, command.Password!, command.RememberMe, lockoutOnFailure: true);

					if (result.Succeeded)
					{
						_logger.LogDebug("SignInCommandHandler: user signed in.");
						authenticationResult = new AuthenticationResult(returnUrl!);

						await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.GetFullName(), clientId: context?.Client.ClientId));
					}
					else if (result.RequiresTwoFactor)
					{
						_logger.LogDebug("SignInCommandHandler: requires two factor.");
						authenticationResult = new AuthenticationResult("SignInTwoStep", new { command.RememberMe, ReturnUrl = returnUrl });
					}
					else if (result.IsLockedOut)
					{
						_logger.LogDebug("SignInCommandHandler: locked out.");
						authenticationResult = new AuthenticationResult("LockedOut", null);

						await _events.RaiseAsync(new UserLoginFailureEvent(command.Email, "Locked Out", clientId: context?.Client.ClientId));
					}
					else if (result.IsNotAllowed)
					{
						_logger.LogDebug("SignInCommandHandler: email not confirmed.");
						authenticationResult = new AuthenticationResult("EmailNotConfimed", new { UserId = user.Id, command.ReturnUrl });

						await _events.RaiseAsync(new UserLoginFailureEvent(command.Email, "Email not confirmed", clientId: context?.Client.ClientId));
					}
					else
					{
						_logger.LogDebug("SignInCommandHandler: email not confirmed.");
						authenticationResult.Errors.Add(new KeyValuePair<string, string>("", "Invalid credentials"));

						await _events.RaiseAsync(new UserLoginFailureEvent(command.Email, "Invalid credentials", clientId: context?.Client.ClientId));
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SignInCommandHandler Error: {Message}", ex.Message);
				throw new AuthenticationException(IdentityProviderConstants.SomethingWentWrongWithAuthentication, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(SignInCommand));
			return authenticationResult;
		}
	}
}
