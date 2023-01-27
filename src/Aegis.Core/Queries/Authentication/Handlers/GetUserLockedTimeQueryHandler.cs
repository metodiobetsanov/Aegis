namespace Aegis.Core.Queries.Authentication.Handlers
{
	using Aegis.Core.Constants;
	using Aegis.Core.Contracts;
	using Aegis.Core.Contracts.CQRS;
	using Aegis.Core.Exceptions;
	using Aegis.Models.Authentication;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Get User Locked Time Query Handler
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.IQueryHandler&lt;Aegis.Core.Queries.Authentication.GetUserLockedTimeQuery, Aegis.Models.Authentication.GetUserLockedTimeQueryResult&gt;" />
	public sealed class GetUserLockedTimeQueryHandler : IQueryHandler<GetUserLockedTimeQuery, GetUserLockedTimeQueryResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<GetUserLockedTimeQueryHandler> _logger;

		/// <summary>
		/// The user manager
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="GetUserLockedTimeQueryHandler" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="userManager">The user manager.</param>
		public GetUserLockedTimeQueryHandler(
			ILogger<GetUserLockedTimeQueryHandler> logger,
			UserManager<AegisUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
		}

		/// <summary>
		/// Handles the specified query.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns><see cref="Aegis.Models.Authentication.GetUserLockedTimeQueryResult"/></returns>
		/// <exception cref="Aegis.Core.Exceptions.IdentityProviderException"></exception>
		public async Task<GetUserLockedTimeQueryResult> Handle(GetUserLockedTimeQuery query, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignInQuery));
			GetUserLockedTimeQueryResult userLockedQueryResult = GetUserLockedTimeQueryResult.Failed();

			try
			{
				if (string.IsNullOrEmpty(query.UserId))
				{
					userLockedQueryResult.AddError("User not found!");
				}
				else
				{
					AegisUser? user = await _userManager.FindByIdAsync(query.UserId);

					if (user is null)
					{
						userLockedQueryResult.AddError("User not found!");
					}
					else
					{
						DateTimeOffset? locked = await _userManager.GetLockoutEndDateAsync(user);
						userLockedQueryResult = GetUserLockedTimeQueryResult.Succeeded(locked);
					}
				}
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "GetUserLockedTimeQueryHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrongWithSignIn, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(SignInQuery));
			return userLockedQueryResult;
		}
	}
}
