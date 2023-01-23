namespace Aegis.Application.Commands.Auth.Handlers
{
	using Aegis.Application.Constants;
	using Aegis.Application.Contracts;
	using Aegis.Application.Contracts.CQRS;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Helpers;
	using Aegis.Models.Auth;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Duende.IdentityServer.Events;
	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// SignIn Two Step Command Handler
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommandHandler&lt;Aegis.Application.Commands.Auth.SignInTwoStepCommand, Aegis.Models.Auth.SignInCommandResult&gt;" />
	public sealed class SignInTwoStepCommandHandler : ICommandHandler<SignInTwoStepCommand, SignInCommandResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<SignInTwoStepCommandHandler> _logger;

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
		/// Initializes a new instance of the <see cref="SignInTwoStepCommandHandler"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="interaction">The interaction.</param>
		/// <param name="events">The events.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public SignInTwoStepCommandHandler(
			ILogger<SignInTwoStepCommandHandler> logger,
			IIdentityServerInteractionService interaction,
			IEventService events,
			SignInManager<AegisUser> signInManager)
		{
			_logger = logger;
			_interaction = interaction;
			_events = events;
			_signInManager = signInManager;
		}

		/// <summary>
		/// Handles the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		///   <see cref="Aegis.Models.Auth.SignInTwoStepCommand" />
		/// </returns>
		/// <exception cref="Aegis.Application.Exceptions.IdentityProviderException"></exception>
		public async Task<SignInCommandResult> Handle(SignInTwoStepCommand command, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignInTwoStepCommand));
			SignInCommandResult signInCommandResult = SignInCommandResult.Failed();

			try
			{
				_logger.LogDebug("SignInTwoStepCommandHandler: get authorization context and validate return URL.");
				AuthorizationRequest? context = await _interaction.GetAuthorizationContextAsync(command.ReturnUrl);
				string returnUrl = context.GetReturnUrl(command.ReturnUrl!);

				_logger.LogDebug("SignInTwoStepCommandHandler: check if user has gone via sign in.");
				AegisUser? user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

				if (user is null)
				{
					_logger.LogDebug("SignInTwoStepCommandHandler: user did not signed in.");
					await _events.RaiseAsync(new UserLoginFailureEvent("", "Invalid credentials", clientId: context?.Client.ClientId));
					signInCommandResult.Errors.Add(new KeyValuePair<string, string>("", "Invalid credentials"));
				}
				else
				{
					SignInResult result = await _signInManager.TwoFactorSignInAsync("Email", command.Code, command.RememberMe, command.RememberClient);

					if (result.Succeeded)
					{
						_logger.LogDebug("SignInTwoStepCommandHandler: user signed in.");
						signInCommandResult = SignInCommandResult.Succeeded(returnUrl!);

						await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.GetFullName(), clientId: context?.Client.ClientId));
					}
					else if (result.IsLockedOut)
					{
						_logger.LogDebug("SignInTwoStepCommandHandler: locked out.");
						signInCommandResult = SignInCommandResult.LockedAccount(user.Id.ToString());

						await _events.RaiseAsync(new UserLoginFailureEvent(user.Email, "Locked Out", clientId: context?.Client.ClientId));
					}
					else if (result.IsNotAllowed)
					{
						_logger.LogDebug("SignInTwoStepCommandHandler: email not confirmed.");
						signInCommandResult = SignInCommandResult.NotActiveAccount(user.Id.ToString());

						await _events.RaiseAsync(new UserLoginFailureEvent(user.Email, "Email not confirmed", clientId: context?.Client.ClientId));
					}
					else
					{
						_logger.LogDebug("SignInTwoStepCommandHandler: email not confirmed.");
						signInCommandResult.Errors.Add(new KeyValuePair<string, string>("", "Wrong Email and/or Password!"));

						await _events.RaiseAsync(new UserLoginFailureEvent(user.Email, "Wrong Email and/or Password!", clientId: context?.Client.ClientId));
					}
				}
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "SignInTwoStepCommandHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrongWithSignIn, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(SignInTwoStepCommand));
			return signInCommandResult;
		}
	}
}
