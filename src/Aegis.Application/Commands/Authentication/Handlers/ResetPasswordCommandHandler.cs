namespace Aegis.Application.Commands.Authentication.Handlers
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;

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
	/// Reset Password Command Handler
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.ICommandHandler&lt;Aegis.Application.Commands.Authentication.ResetPasswordCommand, Aegis.Models.Shared.HandlerResult&gt;" />
	public sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, HandlerResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<ResetPasswordCommandHandler> _logger;

		/// <summary>
		/// The user manager
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResetPasswordCommandHandler" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="userManager">The user manager.</param>
		public ResetPasswordCommandHandler(
			ILogger<ResetPasswordCommandHandler> logger,
			UserManager<AegisUser> userManager)
		{
			_logger = logger;
			_userManager = userManager;
		}

		/// <summary>
		/// Handles the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		/// <exception cref="Aegis.Application.Exceptions.IdentityProviderException"></exception>
		public async Task<HandlerResult> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(ResetPasswordCommand));
			HandlerResult handlerResult = HandlerResult.Succeeded();

			try
			{
				AegisUser? user = await _userManager.FindByIdAsync(command.UserId!);

				if (user is not null)
				{
					IdentityResult emailResult = await _userManager.ResetPasswordAsync(user, command.Token!, command.Password!);

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

			_logger.LogDebug("Handled {name}", nameof(ResetPasswordCommand));
			return handlerResult;
		}
	}
}
