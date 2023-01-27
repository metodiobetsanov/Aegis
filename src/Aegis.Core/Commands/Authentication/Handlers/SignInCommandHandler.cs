#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Commands.Authentication.Handlers
{
	using Aegis.Core.Commands.Authentication;
	using Aegis.Core.Constants;
	using Aegis.Core.Contracts;
	using Aegis.Core.Contracts.CQRS;
	using Aegis.Core.Exceptions;
	using Aegis.Core.Helpers;
	using Aegis.Models.Authentication;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Duende.IdentityServer.Events;
	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// SignIn Command Handler
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.ICommandHandler&lt;Aegis.Core.Commands.Authentication.SignInCommand, Aegis.Models.Authentication.SignInCommandResult&gt;" />
	public sealed class SignInCommandHandler : ICommandHandler<SignInCommand, SignInCommandResult>
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
		/// <returns>
		///  <see cref="Aegis.Models.Authentication.SignInCommandResult" />
		/// </returns>
		/// <exception cref="Aegis.Core.Exceptions.IdentityProviderException"></exception>
		public async Task<SignInCommandResult> Handle(SignInCommand command, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignInCommand));
			SignInCommandResult signInCommandResult = SignInCommandResult.Failed();

			try
			{
				_logger.LogDebug("SignInCommandHandler: get authorization context and validate return URL.");
				AuthorizationRequest? context = await _interaction.GetAuthorizationContextAsync(command.ReturnUrl);
				string returnUrl = context.GetReturnUrl(command.ReturnUrl!);

				_logger.LogDebug("SignInCommandHandler: check if user exists.");
				AegisUser? user = await _userManager.FindByEmailAsync(command.Email!);

				if (user is null)
				{
					_logger.LogDebug("SignInCommandHandler: user does not exists.");
					signInCommandResult.Errors.Add(new KeyValuePair<string, string>("", "Wrong Email and/or Password!"));

					await _events.RaiseAsync(new UserLoginFailureEvent(command.Email, "Wrong Email and/or Password!", clientId: context?.Client.ClientId));
				}
				else
				{
					_logger.LogDebug("SignInCommandHandler: user exists.");
					SignInResult result = await _signInManager.PasswordSignInAsync(user, command.Password!, command.RememberMe, lockoutOnFailure: true);

					if (result.Succeeded)
					{
						_logger.LogDebug("SignInCommandHandler: user signed in.");
						signInCommandResult = SignInCommandResult.Succeeded(returnUrl!);

						await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id.ToString(), user.GetFullName(), clientId: context?.Client.ClientId));
					}
					else if (result.RequiresTwoFactor)
					{
						_logger.LogDebug("SignInCommandHandler: requires two factor.");
						signInCommandResult = SignInCommandResult.TwoStepRequired(user.Id.ToString());
					}
					else if (result.IsLockedOut)
					{
						_logger.LogDebug("SignInCommandHandler: locked out.");
						signInCommandResult = SignInCommandResult.LockedAccount(user.Id.ToString());

						await _events.RaiseAsync(new UserLoginFailureEvent(command.Email, "Locked Out", clientId: context?.Client.ClientId));
					}
					else if (result.IsNotAllowed)
					{
						_logger.LogDebug("SignInCommandHandler: email not confirmed.");
						signInCommandResult = SignInCommandResult.NotActiveAccount(user.Id.ToString());

						await _events.RaiseAsync(new UserLoginFailureEvent(command.Email, "Email not confirmed", clientId: context?.Client.ClientId));
					}
					else
					{
						_logger.LogDebug("SignInCommandHandler: email not confirmed.");
						signInCommandResult.Errors.Add(new KeyValuePair<string, string>("", "Wrong Email and/or Password!"));

						await _events.RaiseAsync(new UserLoginFailureEvent(command.Email, "Wrong Email and/or Password!", clientId: context?.Client.ClientId));
					}
				}
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "SignInCommandHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrongWithSignIn, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(SignInCommand));
			return signInCommandResult;
		}
	}
}
