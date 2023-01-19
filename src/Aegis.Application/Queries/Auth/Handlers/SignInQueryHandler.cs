namespace Aegis.Application.Queries.Auth.Handlers
{
	using Aegis.Application.Constants;
	using Aegis.Application.Contracts.CQRS;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Helpers;
	using Aegis.Models.Shared;

	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using Microsoft.Extensions.Logging;

	/// <summary>
	/// SignInp Query Handler
	/// </summary>
	/// <seealso cref="Chimera.Application.Contracts.CQRS.IQueryHandler&lt;Chimera.Application.Queries.Authentication.SignInQuery, Chimera.Models.Authentication.AuthenticationResult&gt;" />
	public sealed class SignInQueryHandler : IQueryHandler<SignInQuery, AuthenticationResult>
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
		/// <returns></returns>
		public async Task<AuthenticationResult> Handle(SignInQuery query, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignInQuery));
			AuthenticationResult authenticationResult = new AuthenticationResult(false);

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
				authenticationResult = new AuthenticationResult(returnUrl!);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "SignInQueryHandler Error: {Message}", ex.Message);
				throw new AuthenticationException(IdentityProviderConstants.SomethingWentWrongWithAuthentication, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(SignInQuery));
			return authenticationResult;
		}
	}
}
