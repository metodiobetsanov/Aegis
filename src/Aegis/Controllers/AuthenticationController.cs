namespace Aegis.Controllers
{
	using Aegis.Application.Commands.Authentication;
	using Aegis.Application.Helpers;
	using Aegis.Application.Queries.Authentication;
	using Aegis.Application.Validators.Commands.Authentication;
	using Aegis.Exceptions;
	using Aegis.Models.Authentication;
	using Aegis.Models.Shared;

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
	public class AuthenticationController : Controller
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<AuthenticationController> _logger;

		/// <summary>
		/// The mediator
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="mediator">The mediator.</param>
		public AuthenticationController(ILogger<AuthenticationController> logger, IMediator mediator)
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
		public IActionResult SignIn([FromQuery] string? returnUrl)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.SignIn));
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.SignIn));

			if (this.CheckForAuthenticatedUser(returnUrl, out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.SignIn));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.SignIn));
			_logger.LogDebug("GET@{name}: prepare command(transfer query values).", nameof(this.SignIn));
			SignInCommand command = new SignInCommand { ReturnUrl = returnUrl };

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
			_logger.LogDebug("POST@{name}: check if user is authenticated.", nameof(this.SignIn));

			if (this.CheckForAuthenticatedUser(command.ReturnUrl, out string? redirectTo))
			{
				_logger.LogDebug("POST@{name}: user is authenticated, redirecting.", nameof(this.SignIn));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("POST@{name}: user is not authenticated.", nameof(this.SignIn));
			_logger.LogDebug("POST@{name}: validate command.", nameof(this.SignIn));
			SignInCommandValidator validator = new SignInCommandValidator();
			ValidationResult validationresult = validator.Validate(command);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("POST@{name}: command valid.", nameof(this.SignIn));
				_logger.LogDebug("POST@{name}: send command to handler.", nameof(this.SignIn));
				SignInCommandResult result = await _mediator.Send(command);

				if (result.Success)
				{
					_logger.LogDebug("POST@{name}: sign in successful.", nameof(this.SignIn));
					return this.Redirect(result.ReturnUrl!);
				}
				else if (result.RequiresTwoStep)
				{
					_logger.LogDebug("POST@{name}: sign in requires two step.", nameof(this.SignIn));
					return this.RedirectToAction(nameof(this.SignInTwoStep), new { command.RememberMe, command.ReturnUrl });
				}
				else if (result.AccounNotActive)
				{
					_logger.LogDebug("POST@{name}: account is not active.", nameof(this.SignIn));
					return this.RedirectToAction(nameof(this.SendAccountActivation), new { result.UserId });
				}
				else if (result.AccounLocked)
				{
					_logger.LogDebug("POST@{name}: account is locked.", nameof(this.SignIn));
					return this.RedirectToAction("Locked", new { result.UserId });
				}
				else
				{
					_logger.LogDebug("POST@{name}: sign in failed.", nameof(this.SignIn));
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				_logger.LogDebug("POST@{name}: command not valid.", nameof(this.SignIn));
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
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.SignInTwoStep));

			if (this.CheckForAuthenticatedUser(query.ReturnUrl, out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.SignInTwoStep));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.SignInTwoStep));
			_logger.LogDebug("GET@{name}: send query to handler.", nameof(this.SignInTwoStep));
			SignInQueryResult result = await _mediator.Send(query);

			if (!result.Success)
			{
				_logger.LogDebug("GET@{name}: sign in two step failed.", nameof(this.SignInTwoStep));
				result.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("GET@{name}: prepare command(transfer query values).", nameof(this.SignInTwoStep));
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
		public async Task<IActionResult> SignInTwoStep([FromForm] SignInTwoStepCommand command)
		{
			_logger.LogDebug("Executing POST@{name}.", nameof(this.SignInTwoStep));
			_logger.LogDebug("POST@{name}: check if user is authenticated.", nameof(this.SignInTwoStep));

			if (this.CheckForAuthenticatedUser(command.ReturnUrl, out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.SignInTwoStep));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("POST@{name}: user is not authenticated.", nameof(this.SignInTwoStep));
			_logger.LogDebug("POST@{name}: validate command.", nameof(this.SignInTwoStep));
			SignInTwoStepCommandValidator validator = new SignInTwoStepCommandValidator();
			ValidationResult validationresult = validator.Validate(command);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("POST@{name}: command is valid.", nameof(this.SignInTwoStep));
				_logger.LogDebug("POST@{name}: send command to handler.", nameof(this.SignInTwoStep));
				SignInCommandResult result = await _mediator.Send(command);

				if (result.Success)
				{
					_logger.LogDebug("POST@{name}: sign in successful.", nameof(this.SignInTwoStep));
					return this.Redirect(result.ReturnUrl!);
				}
				else if (result.AccounNotActive)
				{
					_logger.LogDebug("POST@{name}: account not active.", nameof(this.SignInTwoStep));
					return this.RedirectToAction(nameof(this.SendAccountActivation), new { result.UserId });
				}
				else if (result.AccounLocked)
				{
					_logger.LogDebug("POST@{name}: account is locked.", nameof(SignInTwoStep));
					return this.RedirectToAction("Locked", new { result.UserId });
				}
				else
				{
					_logger.LogDebug("POST@{name}: sign in failed.", nameof(this.SignInTwoStep));
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				_logger.LogDebug("POST@{name}: command is not valid.", nameof(this.SignInTwoStep));
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
		public IActionResult SignUp([FromQuery] string? returnUrl)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.SignUp));
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.SignUp));

			if (this.CheckForAuthenticatedUser(returnUrl, out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.SignIn));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.SignUp));
			_logger.LogDebug("GET@{name}: prepare command(transfer query values).", nameof(this.SignUp));
			SignUpCommand command = new SignUpCommand { ReturnUrl = returnUrl };

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
			_logger.LogDebug("POST@{name}: check if user is authenticated.", nameof(this.SignUp));

			if (this.CheckForAuthenticatedUser(command.ReturnUrl, out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.SignUp));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("POST@{name}: user is not authenticated.", nameof(this.SignUp));
			_logger.LogDebug("POST@{name}: validate command.", nameof(this.SignUp));
			SignUpCommandValidator validator = new SignUpCommandValidator();
			ValidationResult validationresult = validator.Validate(command);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("POST@{name}: command is valid.", nameof(this.SignUp));
				_logger.LogDebug("POST@{name}: send command to handler.", nameof(this.SignUp));
				SignUpCommandResult result = await _mediator.Send(command);

				if (result.Success)
				{
					_logger.LogDebug("POST@{name}: sign up successful.", nameof(this.SignUp));
					return this.RedirectToAction(nameof(this.SendAccountActivation), new { result.UserId });
				}
				else
				{
					_logger.LogDebug("POST@{name}: sign up failed.", nameof(this.SignUp));
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				_logger.LogDebug("POST@{name}: command is not valid.", nameof(this.SignUp));
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
			_logger.LogDebug("GET@{name}: send query to handler.", nameof(this.SignOut));
			SignOutQueryResult result = await _mediator.Send(query);

			_logger.LogDebug("GET@{name}: prepare command(transfer query values).", nameof(this.SignOut));
			SignOutCommand command = new SignOutCommand { LogoutId = query.LogoutId };

			if (!result.ShowSignoutPrompt)
			{
				_logger.LogDebug("GET@{name}: do not show logout prompt, signing out.", nameof(this.SignOut));
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
			_logger.LogDebug("POST@{name}: send query to handler.", nameof(this.SignOut));
			SignOutCommandResult result = await _mediator.Send(command);

			if (result.Success)
			{
				_logger.LogDebug("POST@{name}: sign out successful.", nameof(this.SignOut));
				return this.Redirect(result.ReturnUrl!);
			}
			else
			{
				_logger.LogDebug("POST@{name}: sign out failed.", nameof(this.SignOut));
				result.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed POST@{name}.", nameof(this.SignOut));
			return this.View(command);
		}

		/// <summary>
		/// Locked.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/Locked")]
		public async Task<IActionResult> Locked([FromQuery] GetUserLockedTimeQuery query)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(Locked));
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.Locked));

			if (this.CheckForAuthenticatedUser("~/", out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.Locked));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.Locked));
			_logger.LogDebug("GET@{name}: send query to handler.", nameof(this.Locked));
			GetUserLockedTimeQueryResult result = await _mediator.Send(query);

			if (result.Success)
			{
				this.ViewBag.LockedFor = Convert.ToInt32(result.LockedTill!.Value.Subtract(DateTime.UtcNow).TotalSeconds);
			}
			else
			{
				this.ViewBag.LockedFor = 600;
				_logger.LogDebug("GET@{name}: get user locked time failed.", nameof(this.Locked));
				result.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed GET@{name}.", nameof(this.Locked));
			return this.View();
		}

		/// <summary>
		/// Emails the confirmation token.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/SendAccountActivation")]
		public async Task<IActionResult> SendAccountActivation([FromQuery] SendAccountActivationCommand command)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.SendAccountActivation));
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.SendAccountActivation));

			if (this.CheckForAuthenticatedUser("~/", out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.SignIn));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.SignIn));
			_logger.LogDebug("GET@{name}: validate command.", nameof(this.SendAccountActivation));
			SendAccountActivationCommandValidator validator = new SendAccountActivationCommandValidator();
			ValidationResult validationresult = validator.Validate(command);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("GET@{name}: command is valid.", nameof(this.SendAccountActivation));
				_logger.LogDebug("GET@{name}: send command to handler.", nameof(this.SendAccountActivation));
				HandlerResult result = await _mediator.Send(command);

				if (!result.Success)
				{
					_logger.LogDebug("GET@{name}: send email confirmation failed.", nameof(this.SendAccountActivation));
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				_logger.LogDebug("GET@{name}: command is not valid.", nameof(this.SendAccountActivation));
				validationresult.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed GET@{name}.", nameof(this.SendAccountActivation));
			return this.View();
		}

		/// <summary>
		/// Confirms the email.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/ActivateAccount")]
		public async Task<IActionResult> ActivateAccount([FromQuery] ActivateAccountCommand query)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.ActivateAccount));
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.ActivateAccount));

			if (this.CheckForAuthenticatedUser("~/", out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.ActivateAccount));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.ActivateAccount));
			_logger.LogDebug("GET@{name}: validate command.", nameof(this.ActivateAccount));
			ActivateAccountCommandValidator validator = new ActivateAccountCommandValidator();
			ValidationResult validationresult = validator.Validate(query);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("GET@{name}: command is valid.", nameof(this.ActivateAccount));
				_logger.LogDebug("GET@{name}: send query to handler.", nameof(this.ActivateAccount));
				HandlerResult result = await _mediator.Send(query);

				if (!result.Success)
				{
					_logger.LogDebug("GET@{name}: confirm email failed.", nameof(this.ActivateAccount));
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				_logger.LogDebug("GET@{name}: command is valid.", nameof(this.ActivateAccount));
				validationresult.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed GET@{name}.", nameof(this.ActivateAccount));
			return this.View();
		}

		/// <summary>
		/// Checks for authenticated user.
		/// </summary>
		/// <param name="returnUrl">The return URL.</param>
		/// <returns></returns>
		private bool CheckForAuthenticatedUser(string? returnUrl, out string? redirectTo)
		{
			if (this.User.IsAuthenticated())
			{
				SignInQueryResult signInResult = _mediator.Send(new SignInQuery { ReturnUrl = returnUrl }).GetAwaiter().GetResult();

				if (signInResult.Success)
				{
					redirectTo = signInResult.ReturnUrl!;
					return true;
				}
				else
				{
					throw new ApplicationFlowException();
				}
			}

			redirectTo = null;
			return false;
		}
	}
}
