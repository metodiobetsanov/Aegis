namespace Aegis.Core.Commands.Authentication.Handlers
{
	using Aegis.Core.Constants;
	using Aegis.Core.Contracts;
	using Aegis.Core.Contracts.CQRS;
	using Aegis.Core.Exceptions;
	using Aegis.Core.Helpers;
	using Aegis.Models.Shared;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;

	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Activate Account Command Handler
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.ICommandHandler&lt;Aegis.Core.Commands.Authentication.ActivateAccountCommand, Aegis.Models.Shared.HandlerResult&gt;" />
	public sealed class ActivateAccountCommandHandler : ICommandHandler<ActivateAccountCommand, HandlerResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<ActivateAccountCommandHandler> _logger;

		/// <summary>
		/// The user manager
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="ActivateAccountCommandHandler" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="userManager">The user manager.</param>
		public ActivateAccountCommandHandler(
			ILogger<ActivateAccountCommandHandler> logger,
			UserManager<AegisUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
		}

		/// <summary>
		/// Handles the specified query.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		///   <see cref="HandlerResult" />
		/// </returns>
		/// <exception cref="IdentityProviderException"></exception>
		public async Task<HandlerResult> Handle(ActivateAccountCommand command, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(ActivateAccountCommand));
			HandlerResult handlerResult = HandlerResult.Succeeded();

			try
			{
				AegisUser? user = await _userManager.FindByIdAsync(command.UserId!);

				if (user is not null)
				{
					IdentityResult emailResult = await _userManager.ConfirmEmailAsync(user, command.Token!);

					if (!emailResult.Succeeded)
					{
						handlerResult = HandlerResult.Failed();
						emailResult.AddToFailedResult(handlerResult);
					}
				}
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "EmailConfirmationQueryHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrong, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(ActivateAccountCommand));
			return handlerResult;
		}
	}
}
