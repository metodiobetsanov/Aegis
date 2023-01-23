namespace Aegis.Application.Validators.Commands.Authentication
{
	using Aegis.Application.Queries.Authentication;

	using FluentValidation;

	/// <summary>
	/// Email Confirmation Query Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Application.Queries.Authentication.EmailConfirmationQuery&gt;" />
	public sealed class EmailConfirmationQueryValidator : AbstractValidator<EmailConfirmationQuery>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EmailConfirmationQueryValidator"/> class.
		/// </summary>
		public EmailConfirmationQueryValidator()
		{
			this.RuleFor(x => x.UserId)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!");
		}
	}
}
