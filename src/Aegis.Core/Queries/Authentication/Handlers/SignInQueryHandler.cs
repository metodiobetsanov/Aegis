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
	using Aegis.Core.Contracts;
	using Aegis.Core.Contracts.CQRS;
	using Aegis.Core.Exceptions;
	using Aegis.Core.Helpers;
	using Aegis.Models.Authentication;
	using Aegis.Models.Shared;

	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using Microsoft.Extensions.Logging;

	using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

	/// <summary>
	/// SignInp Query Handler
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.IQueryHandler&lt;Aegis.Core.Queries.Authentication.SignInQuery, Aegis.Models.Authentication.SignInQueryResult&gt;" />
	public sealed class SignInQueryHandler : IQueryHandler<SignInQuery, SignInQueryResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<SignInQueryHandler> _logger;

		/// <summary>
		/// The interaction.
		/// </summary>
		private readonly IIdentityServerInteractionService _interaction;

		/// <summary>
		/// Initializes a new instance of the <see cref="SignInQueryHandler"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		public SignInQueryHandler(
			ILogger<SignInQueryHandler> logger,
			IIdentityServerInteractionService interaction)
		{
			_logger = logger;
			_interaction = interaction;
		}

		/// <summary>
		/// Handles the specified query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns><see cref="Aegis.Models.Authentication.SignInQueryResult"/></returns>
		/// <exception cref="Aegis.Core.Exceptions.IdentityProviderException"></exception>
		public async Task<SignInQueryResult> Handle(SignInQuery query, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignInQuery));
			SignInQueryResult signInResult = SignInQueryResult.Failed();

			try
			{
				_logger.LogDebug("SignInQueryHandler: get authorization context and validate return URL.");
				AuthorizationRequest? context = await _interaction.GetAuthorizationContextAsync(query.ReturnUrl);
				string returnUrl = context.GetReturnUrl(query.ReturnUrl!);

				_logger.LogDebug("SignInQueryHandler: create result.");
				signInResult = SignInQueryResult.Succeeded(returnUrl!);
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "SignInQueryHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrongWithSignIn, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(SignInQuery));
			return signInResult;
		}
	}
}
