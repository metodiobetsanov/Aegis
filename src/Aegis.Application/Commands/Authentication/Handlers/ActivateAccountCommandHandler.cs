namespace Aegis.Application.Commands.Authentication.Handlers
{
	using Aegis.Application.Constants;
	using Aegis.Application.Contracts;
	using Aegis.Application.Contracts.CQRS;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Helpers;
	using Aegis.Models.Shared;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;

	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Activate Account Command Handler
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommandHandler&lt;Aegis.Application.Commands.Authentication.ActivateAccountCommand, Aegis.Models.Shared.HandlerResult&gt;" />
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
			HandlerResult handlerResult = HandlerResult.Failed();

			try
			{
				AegisUser? user = await _userManager.FindByIdAsync(command.UserId!);

				if (user is null)
				{
					_logger.LogDebug("ConfirmEmailQueryHandler: user does not exists.");
					handlerResult.AddError("User Not Found!");
				}
				else
				{
					IdentityResult emailResult = await _userManager.ConfirmEmailAsync(user, command.Token!);

					if (emailResult.Succeeded)
					{
						handlerResult = HandlerResult.Succeeded();
					}
					else
					{
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
