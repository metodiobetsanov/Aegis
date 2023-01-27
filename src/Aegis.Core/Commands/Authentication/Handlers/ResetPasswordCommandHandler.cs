namespace Aegis.Core.Commands.Authentication.Handlers
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;

	using Aegis.Core.Constants;
	using Aegis.Core.Contracts;
	using Aegis.Core.Contracts.CQRS;
	using Aegis.Core.Events.AuditEvents.IdentityProvider;
	using Aegis.Core.Exceptions;
	using Aegis.Core.Helpers;
	using Aegis.Models.Shared;
	using Aegis.Persistence.Entities.IdentityProvider;

	using MediatR;

	using Microsoft.AspNetCore.Identity;

	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Reset Password Command Handler
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.ICommandHandler&lt;Aegis.Core.Commands.Authentication.ResetPasswordCommand, Aegis.Models.Shared.HandlerResult&gt;" />
	public sealed class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, HandlerResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<ResetPasswordCommandHandler> _logger;

		/// <summary>
		/// The mediator
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// The user manager
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="ResetPasswordCommandHandler" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="mediator">The mediator.</param>
		/// <param name="userManager">The user manager.</param>
		public ResetPasswordCommandHandler(
			ILogger<ResetPasswordCommandHandler> logger,
			IMediator mediator,
			UserManager<AegisUser> userManager)
		{
			_logger = logger;
			_mediator = mediator;
			_userManager = userManager;
		}

		/// <summary>
		/// Handles the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		/// <exception cref="Aegis.Core.Exceptions.IdentityProviderException"></exception>
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

					if (emailResult.Succeeded)
					{
						await _mediator.Publish(new ResetPasswordSucceededAuditEvent(user.Id, "Reset Password"), cancellationToken);
					}
					else
					{
						handlerResult = HandlerResult.Failed();
						emailResult.AddToFailedResult(handlerResult);
						await _mediator.Publish(new ResetPasswordFailedAuditEvent(user.Id, "Failed to Reset Password"), cancellationToken);
					}
				}
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "ResetPasswordCommandHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrong, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(ResetPasswordCommand));
			return handlerResult;
		}
	}
}
