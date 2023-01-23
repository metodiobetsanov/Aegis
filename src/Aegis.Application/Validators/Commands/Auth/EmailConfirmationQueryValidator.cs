namespace Aegis.Application.Validators.Commands.Auth
{
	using Aegis.Application.Queries.Auth;

	using FluentValidation;

	/// <summary>
	/// Email Confirmation Query Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Application.Queries.Auth.EmailConfirmationQuery&gt;" />
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
