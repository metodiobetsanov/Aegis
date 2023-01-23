namespace Aegis.Application.Queries.Auth.Handlers
{
	using Aegis.Application.Constants;
	using Aegis.Application.Constants.Services;
	using Aegis.Application.Contracts;
	using Aegis.Application.Contracts.CQRS;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Helpers;
	using Aegis.Models.Auth;
	using Aegis.Models.Shared;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Duende.IdentityServer.Events;
	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Logging;
	using Microsoft.VisualBasic;

	/// <summary>
	/// SignIn Two Step Query Handler
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQueryHandler&lt;Aegis.Application.Queries.Auth.SignInTwoStepQuery, Aegis.Models.Auth.SignInTwoStepQueryResult&gt;" />
	public sealed class SignInTwoStepQueryHandler : IQueryHandler<SignInTwoStepQuery, SignInTwoStepQueryResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<SignInTwoStepQueryHandler> _logger;

		/// <summary>
		/// The interaction
		/// </summary>
		private readonly IIdentityServerInteractionService _interaction;

		/// <summary>
		/// The mail sender
		/// </summary>
		private readonly IMailSenderService _mailSender;

		/// <summary>
		/// The events
		/// </summary>
		private readonly IEventService _events;

		/// <summary>
		/// The user manager
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// The sign in manager
		/// </summary>
		private readonly SignInManager<AegisUser> _signInManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInTwoStepQueryHandler" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="interaction">The interaction.</param>
		/// <param name="mailSender">The mail sender.</param>
		/// <param name="events">The events.</param>
		/// <param name="userManager">The user manager.</param>
		/// <param name="signInManager">The sign in manager.</param>
		public SignInTwoStepQueryHandler(
			ILogger<SignInTwoStepQueryHandler> logger,
			IIdentityServerInteractionService interaction,
			IMailSenderService mailSender,
			IEventService events,
			UserManager<AegisUser> userManager,
			SignInManager<AegisUser> signInManager)
		{
			_logger = logger;
			_interaction = interaction;
			_mailSender = mailSender;
			_events = events;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		/// <summary>
		/// Handles the specified query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns><see cref="Aegis.Models.Auth.SignInQuerySignInTwoStepQueryResultResult"/></returns>
		/// <exception cref="Aegis.Application.Exceptions.IdentityProviderException"></exception>
		public async Task<SignInTwoStepQueryResult> Handle(SignInTwoStepQuery query, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignInTwoStepQuery));
			SignInTwoStepQueryResult signInResult = SignInTwoStepQueryResult.Failed();

			try
			{
				_logger.LogDebug("SignInQueryHandler: get authorization context and validate return URL.");
				AuthorizationRequest? context = await _interaction.GetAuthorizationContextAsync(query.ReturnUrl);
				string returnUrl = context.GetReturnUrl(query.ReturnUrl!);

				_logger.LogDebug("SignInTwoStepCommandHandler: check if user has gone via sign in.");
				AegisUser? user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

				if (user is null)
				{
					_logger.LogDebug("SignInTwoStepCommandHandler: user did not signed in.");
					await _events.RaiseAsync(new UserLoginFailureEvent("", "Invalid credentials"));
					signInResult.Errors.Add(new KeyValuePair<string, string>("", "Invalid credentials"));
				}
				else
				{
					string token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
					await _mailSender.SendVerificationCodeAsync(token, user.Email!);
				}

				signInResult = SignInTwoStepQueryResult.Succeeded(returnUrl!);
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "SignInQueryHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrongWithSignIn, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(SignInTwoStepQuery));
			return signInResult;
		}
	}
}
