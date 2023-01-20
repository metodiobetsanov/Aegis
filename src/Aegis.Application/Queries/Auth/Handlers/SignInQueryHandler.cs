namespace Aegis.Application.Queries.Auth.Handlers
{
	using Aegis.Application.Constants;
	using Aegis.Application.Contracts;
	using Aegis.Application.Contracts.CQRS;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Helpers;
	using Aegis.Models.Auth;
	using Aegis.Models.Shared;

	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using Microsoft.Extensions.Logging;

	/// <summary>
	/// SignInp Query Handler
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQueryHandler&lt;Aegis.Application.Queries.Auth.SignInQuery, Aegis.Models.Auth.SignInQueryResult&gt;" />
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
		/// <returns><see cref="Aegis.Models.Auth.SignInQueryResult"/></returns>
		/// <exception cref="Aegis.Application.Exceptions.IdentityProviderException"></exception>
		public async Task<SignInQueryResult> Handle(SignInQuery query, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignInQuery));
			SignInQueryResult signInResult = SignInQueryResult.Failed();

			try
			{
				_logger.LogDebug("SignInQueryHandler: get authorization context and validate return URL.");
				AuthorizationRequest? context = await _interaction.GetAuthorizationContextAsync(query.ReturnUrl);
				string? returnUrl;

				if (context == null)
				{
					returnUrl = IdentityServerHelpers.GetReturnUrl(query.ReturnUrl);
				}
				else
				{
					returnUrl = query.ReturnUrl;
				}

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
