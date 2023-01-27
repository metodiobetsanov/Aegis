namespace Aegis.Core.Services
{
	using System.Threading.Tasks;

	using Aegis.Core.Constants;
	using Aegis.Core.Constants.Services;
	using Aegis.Core.Exceptions;
	using Aegis.Models.Settings;

	using Microsoft.Extensions.Logging;

	using SendGrid;
	using SendGrid.Helpers.Mail;

	/// <summary>
	/// Mail Sender Service
	/// </summary>
	/// <seealso cref="Chimera.Application.Constants.Services.IMailSenderService" />
	public sealed class MailSenderService : IMailSenderService
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<MailSenderService> _logger;

		/// <summary>
		/// The send grid settings
		/// </summary>
		private readonly SendGridSettings _sendGridSettings;

		/// <summary>
		/// The client
		/// </summary>
		private readonly SendGridClient _client;

		/// <summary>
		/// no-replay@mnb.software
		/// </summary>
		private readonly EmailAddress _from = new EmailAddress(ApplicationConstants.ApplicationEmail, ApplicationConstants.ApplicationName);

		/// <summary>
		/// Initializes a new instance of the <see cref="MailSenderService"/> class.
		/// </summary>
		/// <param name="sendGridSettings">The send grid settings.</param>
		public MailSenderService(ILogger<MailSenderService> logger, SendGridSettings sendGridSettings)
		{
			_logger = logger;
			_sendGridSettings = sendGridSettings;

			_client ??= new SendGridClient(_sendGridSettings.ApiKey);
		}

		/// <summary>
		/// Sends the password reset link.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <param name="recipient">The recipient.</param>
		/// <returns></returns>
		public async Task SendResetPasswordLinkAsync(string link, string recipient)
		{
			_logger.LogDebug("Executing SendEmailConfirmationLinkAsync");
			_logger.LogDebug("SendEmailConfirmationLinkAsync: send code to {recipient}", recipient);
			EmailAddress to = new EmailAddress(recipient);

			_logger.LogDebug("SendEmailConfirmationLinkAsync: create template data object");
			object templateData = new { link };

			_logger.LogDebug("SendVerificationCodeAsync: send email template");
			await this.SendEmailTemplate(to, _sendGridSettings.ResetPasswordTemplate, templateData);

			_logger.LogDebug("Executed SendEmailConfirmationLinkAsync");
		}

		/// <summary>
		/// Sends the email confirmation link.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <param name="recipient">The recipient.</param>
		public async Task SendEmailConfirmationLinkAsync(string link, string recipient)
		{
			_logger.LogDebug("Executing SendEmailConfirmationLinkAsync");
			_logger.LogDebug("SendEmailConfirmationLinkAsync: send code to {recipient}", recipient);
			EmailAddress to = new EmailAddress(recipient);

			_logger.LogDebug("SendEmailConfirmationLinkAsync: create template data object");
			object templateData = new { link };

			_logger.LogDebug("SendVerificationCodeAsync: send email template");
			await this.SendEmailTemplate(to, _sendGridSettings.EmailConfirmationTemplate, templateData);

			_logger.LogDebug("Executed SendEmailConfirmationLinkAsync");
		}

		/// <summary>
		/// Sends the verification code.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="recipient">The recipient.</param>
		public async Task SendVerificationCodeAsync(string code, string recipient)
		{
			_logger.LogDebug("Executing SendVerificationCodeAsync");
			_logger.LogDebug("SendVerificationCodeAsync: send code to {recipient}", recipient);
			EmailAddress to = new EmailAddress(recipient);

			_logger.LogDebug("SendVerificationCodeAsync: create template data object");
			object templateData = new { code };

			_logger.LogDebug("SendVerificationCodeAsync: send email template");
			await this.SendEmailTemplate(to, _sendGridSettings.VerificationCodeTemplate, templateData);

			_logger.LogDebug("Executed SendVerificationCodeAsync");
		}

		/// <summary>
		/// Sends the email template.
		/// </summary>
		/// <param name="to">To.</param>
		/// <param name="template">The template.</param>
		/// <param name="templateData">The template data.</param>
		/// <exception cref="System.Exception">Failed to send email</exception>
		private async Task SendEmailTemplate(EmailAddress to, string template, object templateData)
		{
			_logger.LogDebug("Executing SendEmailTempalate");

			try
			{
				_logger.LogDebug("SendEmailTempalate: create message");
				SendGridMessage msg = MailHelper.CreateSingleTemplateEmail(_from, to, template, templateData);

				_logger.LogDebug("SendEmailTempalate: send message");
				Response result = await _client.SendEmailAsync(msg);

				if (!result.IsSuccessStatusCode)
				{
					_logger.LogError("Failed to send email, Received {StatusCode}", result.StatusCode);
					throw new ServiceException("Failed to send email");
				}
			}
			catch (Exception ex) when (ex is not ServiceException)
			{
				throw new ServiceException(ex.Message, ex);
			}

			_logger.LogDebug("Executed SendEmailTempalate");
		}
	}
}
