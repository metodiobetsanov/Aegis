namespace Aegis.Application.Validators.Commands.Authentication
{
	using Aegis.Application.Queries.Authentication;

	using FluentValidation;

	/// <summary>
	/// Confirm Email Query Validator
	/// </summary>
	/// <seealso cref="FluentValidation.AbstractValidator&lt;Aegis.Application.Queries.Authentication.ConfirmEmailQuery&gt;" />
	public sealed class ConfirmEmailQueryValidator : AbstractValidator<ConfirmEmailQuery>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConfirmEmailQueryValidator"/> class.
		/// </summary>
		public ConfirmEmailQueryValidator()
		{
			this.RuleFor(x => x.UserId)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!");

			this.RuleFor(x => x.Token)
				.NotNull().WithMessage("This field is required!")
				.NotEmpty().WithMessage("This field is required!");
		}
	}
}
