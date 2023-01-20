namespace Aegis.Application.Queries.Auth.Handlers
{
	using Aegis.Application.Commands.Auth;
	using Aegis.Application.Commands.Auth.Handlers;
	using Aegis.Application.Constants;
	using Aegis.Application.Constants.Services;
	using Aegis.Application.Contracts;
	using Aegis.Application.Contracts.CQRS;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Queries.Auth;
	using Aegis.Models.Auth;
	using Aegis.Models.Settings;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Identity;

	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Email Confirmation Query Handler
	/// </summary>
	/// <seealso cref="Aegis.Application.Contracts.CQRS.IQueryHandler&lt;Aegis.Application.Queries.Auth.EmailConfirmationQuery, Aegis.Models.Auth.EmailConfirmationQueryResult&gt;" />
	public sealed class EmailConfirmationQueryHandler : IQueryHandler<EmailConfirmationQuery, EmailConfirmationQueryResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<EmailConfirmationQueryHandler> _logger;

		/// <summary>
		/// The mail sender service
		/// </summary>
		private readonly IMailSenderService _mailSenderService;

		/// <summary>
		/// The application settings
		/// </summary>
		private readonly AppSettings _appSettings;

		/// <summary>
		/// The user manager
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="EmailConfirmationQueryHandler" /> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="mailSenderService">The mail sender service.</param>
		/// <param name="appSettings">The application settings.</param>
		/// <param name="userManager">The user manager.</param>
		public EmailConfirmationQueryHandler(
			ILogger<EmailConfirmationQueryHandler> logger,
			IMailSenderService mailSenderService,
			AppSettings appSettings,
			UserManager<AegisUser> userManager)
		{
			_logger = logger;
			_mailSenderService = mailSenderService;
			_appSettings = appSettings;
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
		public async Task<EmailConfirmationQueryResult> Handle(EmailConfirmationQuery query, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(EmailConfirmationQuery));
			EmailConfirmationQueryResult sendConfirmationEmailCommandResult = EmailConfirmationQueryResult.Failed();

			try
			{
				_logger.LogDebug("EmailConfirmationQueryHandler: check if user exists.");
				AegisUser? user = await _userManager.FindByIdAsync(query.UserId!);

				if (user is null)
				{
					_logger.LogDebug("EmailConfirmationQueryHandler: user does not exists.");
					sendConfirmationEmailCommandResult.Errors.Add(new KeyValuePair<string, string>("", "User Not Found!"));
				}
				else
				{
					string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

					QueryString res = QueryString.Create(new List<KeyValuePair<string, string?>>
					{
						new KeyValuePair<string, string?>("UserId", user.Id.ToString()),
						new KeyValuePair<string, string?>("Token", token)
					});

					string link = $"https://{_appSettings.PublicDomain}/Auth/ConfirmEmail{res}";
					await _mailSenderService.SendEmailConfirmationLinkAsync(link, user.Email!);
					sendConfirmationEmailCommandResult = EmailConfirmationQueryResult.Succeeded();
				}
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "EmailConfirmationQueryHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrong, ex.Message, ex);
			}

			_logger.LogDebug("Handled {name}", nameof(EmailConfirmationQuery));
			return sendConfirmationEmailCommandResult;
		}
	}
}
