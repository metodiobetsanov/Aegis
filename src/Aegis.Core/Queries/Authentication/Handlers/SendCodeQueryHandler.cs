#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Queries.Authentication.Handlers
{
	using Aegis.Core.Constants;
	using Aegis.Core.Constants.Services;
	using Aegis.Core.Contracts;
	using Aegis.Core.Contracts.CQRS;
	using Aegis.Core.Exceptions;
	using Aegis.Core.Hubs;
	using Aegis.Models.Shared;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.SignalR;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Send Code Query Handler
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.IQueryHandler&lt;Aegis.Core.Queries.Authentication.SendCodeQuery, Aegis.Models.Shared.HandlerResult&gt;" />
	public sealed class SendCodeQueryHandler : IQueryHandler<SendCodeQuery, HandlerResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<SendCodeQueryHandler> _logger;

		/// <summary>
		/// The mail sender
		/// </summary>
		private readonly IMailSenderService _mailSender;

		/// <summary>
		/// The user manager
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="SendCodeQueryHandler" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="mailSender">The mail sender.</param>
		/// <param name="userManager">The user manager.</param>
		public SendCodeQueryHandler(
			ILogger<SendCodeQueryHandler> logger,
			IMailSenderService mailSender,
			UserManager<AegisUser> userManager)
		{
			_logger = logger;
			_mailSender = mailSender;
			_userManager = userManager;
		}

		/// <summary>
		/// Handles the specified query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns><see cref="Aegis.Models.Authentication.SignInQuerySignInTwoStepQueryResultResult"/></returns>
		/// <exception cref="Aegis.Core.Exceptions.IdentityProviderException"></exception>
		public async Task<HandlerResult> Handle(SendCodeQuery query, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignInTwoStepQuery));
			HandlerResult signInResult = HandlerResult.Succeeded();

			try
			{
				_logger.LogDebug("SendCodeQueryHandler: check if user has gone via sign in.");
				AegisUser? user = await _userManager.FindByIdAsync(query.UserId!);

				if (user is not null)
				{
					string token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
					await _mailSender.SendVerificationCodeAsync(token, user.Email!);
				}
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
