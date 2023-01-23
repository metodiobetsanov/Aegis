namespace Aegis.Application.Queries.Authentication.Handlers
{
	using Aegis.Application.Constants;
	using Aegis.Application.Contracts;
	using Aegis.Application.Contracts.CQRS;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Queries.Authentication;
	using Aegis.Models.Authentication;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;

	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Email Confirmation Query Handler
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQueryHandler&lt;Aegis.Application.Queries.Authentication.ConfirmEmailQuery, Aegis.Models.Authentication.EmailConfirmationQueryResult&gt;" />
	public sealed class ConfirmEmailQueryHandler : IQueryHandler<ConfirmEmailQuery, EmailConfirmationQueryResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<ConfirmEmailQueryHandler> _logger;

		/// <summary>
		/// The user manager
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfirmEmailQueryHandler" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="userManager">The user manager.</param>
		public ConfirmEmailQueryHandler(
			ILogger<ConfirmEmailQueryHandler> logger,
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
		/// <returns>
		///   <see cref="EmailConfirmationQueryResult" />
		/// </returns>
		/// <exception cref="IdentityProviderException"></exception>
		public async Task<EmailConfirmationQueryResult> Handle(ConfirmEmailQuery query, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(ConfirmEmailQuery));
			EmailConfirmationQueryResult sendConfirmationEmailCommandResult = EmailConfirmationQueryResult.Failed();

			try
			{
				AegisUser? user = await _userManager.FindByIdAsync(query.UserId!);

				if (user is null)
				{
					_logger.LogDebug("ConfirmEmailQueryHandler: user does not exists.");
					sendConfirmationEmailCommandResult.Errors.Add(new KeyValuePair<string, string>("", "User Not Found!"));
				}
				else
				{
					IdentityResult emailResult = await _userManager.ConfirmEmailAsync(user, query.Token!);

					if (emailResult.Succeeded)
					{
						sendConfirmationEmailCommandResult = EmailConfirmationQueryResult.Succeeded();
					}
					else
					{
						sendConfirmationEmailCommandResult.Errors.AddRange(
							emailResult.Errors.Select(e => new KeyValuePair<string, string>(e.Code, e.Description)));
					}
				}
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "EmailConfirmationQueryHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrong, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(ConfirmEmailQuery));
			return sendConfirmationEmailCommandResult;
		}
	}
}
