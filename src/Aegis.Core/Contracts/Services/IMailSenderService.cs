namespace Aegis.Core.Constants.Services
{
	/// <summary>
	/// Mail Sender Service interface
	/// </summary>
	public interface IMailSenderService
	{
		/// <summary>
		/// Sends the password reset link.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <param name="recipient">The recipient.</param>
		/// <returns></returns>
		Task SendResetPasswordLinkAsync(string link, string recipient);

		/// <summary>
		/// Sends the email confirmation link.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <param name="recipient">The recipient.</param>
		/// <returns></returns>
		Task SendEmailConfirmationLinkAsync(string link, string recipient);

		/// <summary>
		/// Sends the verification code.
		/// </summary>
		/// <param name="code">The code.</param>
		/// <param name="recipient">The recipient.</param>
		/// <returns></returns>
		Task SendVerificationCodeAsync(string code, string recipient);
	}
}
