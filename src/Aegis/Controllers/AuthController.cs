namespace Aegis.Controllers
{
	using Aegis.Application.Commands.Auth;
	using Aegis.Application.Helpers;
	using Aegis.Application.Queries.Auth;
	using Aegis.Application.Validators.Commands.Auth;
	using Aegis.Models.Auth;

	using Duende.IdentityServer.Extensions;

	using FluentValidation.AspNetCore;
	using FluentValidation.Results;

	using MediatR;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	/// <summary>
	/// Authentication Controller
	/// </summary>
	/// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
	public class AuthController : Controller
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<AuthController> _logger;

		/// <summary>
		/// The mediator
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="mediator">The mediator.</param>
		public AuthController(ILogger<AuthController> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
		}

		/// <summary>
		/// Sign in.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/SignIn")]
		public async Task<IActionResult> SignIn([FromQuery] SignInQuery query)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.SignIn));
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.SignIn));

			if (this.User.IsAuthenticated())
			{
				_logger.LogDebug("GET@{name}: user is authenticated.", nameof(this.SignIn));
				SignInQueryResult signInResult = await _mediator.Send(query);

				if (signInResult.Success)
				{
					return this.Redirect(signInResult.ReturnUrl!);
				}
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.SignIn));
			SignInCommand command = new SignInCommand { ReturnUrl = query.ReturnUrl };

			_logger.LogDebug("Executed GET@{name}.", nameof(this.SignIn));
			return this.View(command);
		}

		/// <summary>
		/// Sign in.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		[HttpPost("/SignIn")]
		public async Task<IActionResult> SignIn([FromForm] SignInCommand command)
		{
			_logger.LogDebug("Executing POST@{name}.", nameof(SignIn));
			_logger.LogDebug("POST@{name}: validate command.", nameof(this.SignIn));
			SignInCommandValidator validator = new SignInCommandValidator();
			ValidationResult validationresult = validator.Validate(command);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("POST@{name}: send command to handler.", nameof(this.SignIn));
				SignInCommandResult result = await _mediator.Send(command);

				if (result.Success)
				{
					return this.Redirect(result.ReturnUrl!);
				}
				else if (result.RequiresTwoStep)
				{
					return this.RedirectToAction(nameof(this.SignInTwoStep), new { command.RememberMe, command.ReturnUrl });
				}
				else if (result.AccounNotActive)
				{
					return this.RedirectToAction(nameof(this.EmailConfirmation), new { result.UserId });
				}
				else if (result.AccounLocked)
				{
					return this.RedirectToAction("Locked", new { result.UserId });
				}
				else
				{
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				validationresult.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed POST@{name}.", nameof(this.SignIn));
			return this.View(command);
		}

		/// <summary>
		/// Sign in Two Step.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/SignInTwoStep")]
		public async Task<IActionResult> SignInTwoStep([FromQuery] SignInTwoStepQuery query)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.SignInTwoStep));

			SignInTwoStepQueryResult result = await _mediator.Send(query);

			if (!result.Success)
			{
				result.AddToModelState(this.ModelState);
			}

			SignInTwoStepCommand command = new SignInTwoStepCommand
			{
				RememberMe = query.RememberMe,
				ReturnUrl = query.ReturnUrl
			};

			_logger.LogDebug("Executed GET@{name}.", nameof(this.SignInTwoStep));
			return this.View(command);
		}

		/// <summary>
		/// Sign in Two Step.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("/SignInTwoStep")]
		public async Task<IActionResult> SignInTwoStep([FromBody] SignInTwoStepCommand command)
		{
			_logger.LogDebug("Executing Post@{name}.", nameof(this.SignInTwoStep));

			SignInTwoStepCommandValidator validator = new SignInTwoStepCommandValidator();
			ValidationResult validationresult = validator.Validate(command);

			if (validationresult.IsValid)
			{
				SignInCommandResult result = await _mediator.Send(command);

				if (result.Success)
				{
					return this.Redirect(result.ReturnUrl!);
				}
				else if (result.AccounNotActive)
				{
					return this.RedirectToAction(nameof(this.EmailConfirmation), new { result.UserId });
				}
				else if (result.AccounLocked)
				{
					return this.RedirectToAction("Locked", new { result.UserId });
				}
				else
				{
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				validationresult.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed Post@{name}.", nameof(this.SignInTwoStep));
			return this.View(command);
		}

		/// <summary>
		/// Signs up.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/SignUp")]
		public async Task<IActionResult> SignUp([FromQuery] SignInQuery query)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.SignUp));
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.SignUp));

			if (this.User.IsAuthenticated())
			{
				_logger.LogDebug("GET@{name}: user is authenticated.", nameof(this.SignUp));
				SignInQueryResult signInResult = await _mediator.Send(query);

				if (signInResult.Success)
				{
					return this.Redirect(signInResult.ReturnUrl!);
				}
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.SignUp));
			SignUpCommand command = new SignUpCommand { ReturnUrl = query.ReturnUrl };

			_logger.LogDebug("Executed GET@{name}.", nameof(this.SignUp));
			return this.View(command);
		}

		/// <summary>
		/// Signs up.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		[HttpPost("/SignUp")]
		public async Task<IActionResult> SignUp([FromForm] SignUpCommand command)
		{
			_logger.LogDebug("Executing POST@{name}.", nameof(SignUp));
			_logger.LogDebug("POST@{name}: validate command.", nameof(this.SignUp));
			SignUpCommandValidator validator = new SignUpCommandValidator();
			ValidationResult validationresult = validator.Validate(command);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("POST@{name}: send command to handler.", nameof(this.SignUp));
				SignUpCommandResult result = await _mediator.Send(command);

				if (result.Success)
				{
					return this.RedirectToAction(nameof(this.EmailConfirmation), new { result.UserId });
				}
				else
				{
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				validationresult.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed POST@{name}.", nameof(this.SignUp));
			return this.View(command);
		}

		/// <summary>
		/// Sign out.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		[Authorize]
		[HttpGet("/SignOut")]
		public async Task<IActionResult> SignOut([FromQuery] SignOutQuery query)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.SignOut));
			SignOutCommand command = new SignOutCommand { LogoutId = query.LogoutId };

			SignOutQueryResult result = await _mediator.Send(query);

			if (!result.ShowSignoutPrompt)
			{
				return await this.SignOut(command);
			}

			_logger.LogDebug("Executed GET@{name}.", nameof(this.SignOut));
			return this.View(command);
		}

		/// <summary>
		/// Sign out.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		[Authorize]
		[ValidateAntiForgeryToken]
		[HttpPost("/SignOut")]
		public async Task<IActionResult> SignOut([FromForm] SignOutCommand command)
		{
			_logger.LogDebug("Executing POST@{name}.", nameof(this.SignOut));
			SignOutCommandResult result = await _mediator.Send(command);

			if (result.Success)
			{
				return this.Redirect(result.ReturnUrl!);
			}
			else
			{
				result.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed POST@{name}.", nameof(this.SignOut));
			return this.View(command);
		}

		/// <summary>
		/// Emails the confirmation token.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/EmailConfirmation")]
		public async Task<IActionResult> EmailConfirmation([FromQuery] EmailConfirmationQuery query)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.EmailConfirmation));
			_logger.LogDebug("GET@{name}: validate query.", nameof(this.EmailConfirmation));
			EmailConfirmationQueryValidator validator = new EmailConfirmationQueryValidator();
			ValidationResult validationresult = validator.Validate(query);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("GET@{name}: send query to handler.", nameof(this.EmailConfirmation));
				EmailConfirmationQueryResult result = await _mediator.Send(query);

				if (!result.Success)
				{
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				validationresult.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed GET@{name}.", nameof(this.EmailConfirmation));
			return this.View();
		}

		/// <summary>
		/// Confirms the email.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/ConfirmEmail")]
		public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.ConfirmEmail));
			_logger.LogDebug("GET@{name}: validate query.", nameof(this.ConfirmEmail));
			ConfirmEmailQueryValidator validator = new ConfirmEmailQueryValidator();
			ValidationResult validationresult = validator.Validate(query);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("GET@{name}: send query to handler.", nameof(this.ConfirmEmail));
				EmailConfirmationQueryResult result = await _mediator.Send(query);

				if (!result.Success)
				{
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				validationresult.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed GET@{name}.", nameof(this.ConfirmEmail));
			return this.View();
		}
	}
}
