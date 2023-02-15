#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Controllers
{
	using Aegis.Core.Commands.Authentication;
	using Aegis.Core.Constants;
	using Aegis.Core.Helpers;
	using Aegis.Core.Queries.Authentication;
	using Aegis.Core.Validators.Commands.Authentication;
	using Aegis.Exceptions;
	using Aegis.Models.Authentication;
	using Aegis.Models.Shared;

	using Duende.IdentityServer.Extensions;

	using FluentValidation;
	using FluentValidation.AspNetCore;
	using FluentValidation.Results;

	using MediatR;

	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.DataProtection;
	using Microsoft.AspNetCore.Mvc;

	using Newtonsoft.Json;

	using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
		/// The data protection provider
		/// </summary>
		private readonly IDataProtectionProvider _dataProtectionProvider;

		/// <summary>
		/// The mediator
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// Initializes a new instance of the <see cref="AuthenticationController"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="mediator">The mediator.</param>
		public AuthenticationController(
			ILogger<AuthenticationController> logger,
			IDataProtectionProvider dataProtectionProvider,
			IMediator mediator)
		{
			_logger = logger;
			_dataProtectionProvider = dataProtectionProvider;
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
					_ = await _mediator.Send(new SendAccountActivationCommand { UserId = result.UserId });
					this.ViewBag.AccountNotActive = "Account is not active!";
					return this.View("ActivateAccountMail");
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
					_ = await _mediator.Send(new SendAccountActivationCommand { UserId = result.UserId });
					this.ViewBag.AccountNotActive = "Account is not active!";
					return this.View("ActivateAccountMail");
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
		/// Sends the code.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/SendCode")]
		public async Task<IActionResult> SendCode([FromQuery] SendCodeQuery query)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.SendCode));
			_logger.LogDebug("GET@{name}: check if user id is provided.", nameof(this.SendCode));

			if (string.IsNullOrEmpty(query.UserId))
			{
				_logger.LogDebug("GET@{name}: user id is not provided, creating query for current user.", nameof(this.SendCode));
				query = new SendCodeQuery { UserId = this.User.Identity.GetSubjectId() };
			}

			_logger.LogDebug("GET@{name}: send query to handler.", nameof(this.SendCode));
			BaseResult result = await _mediator.Send(query);

			if (!result.Success)
			{
				_logger.LogDebug("GET@{name}: send code failed.", nameof(this.SendCode));
				return this.BadRequest(result.Errors);
			}

			_logger.LogDebug("Executed GET@{name}.", nameof(this.SendCode));
			return this.Ok();
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
					_ = await _mediator.Send(new SendAccountActivationCommand { UserId = result.UserId });
					this.ViewBag.AccountNotActive = "Thank you for Signing Up!";
					return this.View("ActivateAccountMail");
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

			if (!result.ShowSignoutPrompt)
			{
				_logger.LogDebug("GET@{name}: do not show logout prompt, signing out.", nameof(this.SignOut));
				return await this.SignOut(new SignOutCommand
				{
					LogoutId = query.LogoutId,
					ForgetClient = true,
					SignOutAllSessions = false
				});
			}

			_logger.LogDebug("GET@{name}: prepare command(transfer query values).", nameof(this.SignOut));
			SignOutCommand command = new SignOutCommand { LogoutId = query.LogoutId };
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
		/// Confirms the email.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/ActivateAccount")]
		public async Task<IActionResult> ActivateAccount([FromQuery] string? pqs)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.ActivateAccount));
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.ActivateAccount));

			if (this.CheckForAuthenticatedUser("~/", out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.ActivateAccount));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.ActivateAccount));
			_logger.LogDebug("GET@{name}: unprotect query string and get the command.", nameof(this.ActivateAccount));

			if (!string.IsNullOrEmpty(pqs))
			{
				ActivateAccountCommand? command = ProtectorHelpers.UnProtectQueryString<ActivateAccountCommand>(
					_dataProtectionProvider.CreateProtector(ProtectorHelpers.QueryStringProtector), pqs);

				if (command is not null)
				{
					_logger.LogDebug("GET@{name}: validate command.", nameof(this.ActivateAccount));
					ActivateAccountCommandValidator validator = new ActivateAccountCommandValidator();
					ValidationResult validationresult = validator.Validate(command);

					if (validationresult.IsValid)
					{
						_logger.LogDebug("GET@{name}: command is valid.", nameof(this.ActivateAccount));
						_logger.LogDebug("GET@{name}: send query to handler.", nameof(this.ActivateAccount));
						_ = await _mediator.Send(command);
					}
				}
			}

			_logger.LogDebug("Executed GET@{name}.", nameof(this.ActivateAccount));
			return this.View("ActivateAccountConfirmation");
		}

		/// <summary>
		/// Emails the Reset Password.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/ForgotPassword")]
		public IActionResult ForgotPassword()
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.ForgotPassword));
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.ForgotPassword));

			if (this.CheckForAuthenticatedUser("~/", out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.ForgotPassword));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.ForgotPassword));
			_logger.LogDebug("GET@{name}: prepare command.", nameof(this.ForgotPassword));
			SendForgetPasswordCommand command = new SendForgetPasswordCommand();

			_logger.LogDebug("Executed GET@{name}.", nameof(this.ForgotPassword));
			return this.View(command);
		}

		/// <summary>
		/// Emails the Reset Password.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("/ForgotPassword")]
		public async Task<IActionResult> ForgotPassword([FromForm] SendForgetPasswordCommand command)
		{
			_logger.LogDebug("Executing POST@{name}.", nameof(ForgotPassword));
			_logger.LogDebug("POST@{name}: check if user is authenticated.", nameof(this.ForgotPassword));

			if (this.CheckForAuthenticatedUser("~/", out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.ForgotPassword));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("POST@{name}: user is not authenticated.", nameof(this.ForgotPassword));
			_logger.LogDebug("POST@{name}: validate command.", nameof(this.ForgotPassword));
			SendForgetPasswordCommandValidator validator = new SendForgetPasswordCommandValidator();
			ValidationResult validationresult = validator.Validate(command);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("POST@{name}: command is valid.", nameof(this.ForgotPassword));
				_logger.LogDebug("POST@{name}: send command to handler.", nameof(this.ForgotPassword));
				_ = await _mediator.Send(command);
			}

			_logger.LogDebug("Executed POST@{name}.", nameof(this.SignUp));
			return this.View("ForgotPasswordMail");
		}

		/// <summary>
		/// Emails the Reset Password.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpGet("/ResetPassword")]
		public IActionResult ResetPassword([FromQuery] string? pqs)
		{
			_logger.LogDebug("Executing GET@{name}.", nameof(this.ResetPassword));
			_logger.LogDebug("GET@{name}: check if user is authenticated.", nameof(this.ResetPassword));

			if (this.CheckForAuthenticatedUser("~/", out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.ResetPassword));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("GET@{name}: user is not authenticated.", nameof(this.ResetPassword));
			_logger.LogDebug("GET@{name}: unprotect query string and get the command.", nameof(this.ActivateAccount));
			ResetPasswordCommand? command = null;

			if (!string.IsNullOrEmpty(pqs))
			{
				command = ProtectorHelpers.UnProtectQueryString<ResetPasswordCommand>(
					_dataProtectionProvider.CreateProtector(ProtectorHelpers.QueryStringProtector), pqs);
			}

			_logger.LogDebug("Executed GET@{name}.", nameof(this.ResetPassword));
			return this.View(command ?? new ResetPasswordCommand());
		}

		/// <summary>
		/// Emails the Reset Password.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		[AllowAnonymous]
		[HttpPost("/ResetPassword")]
		public async Task<IActionResult> ResetPassword([FromForm] ResetPasswordCommand command)
		{
			_logger.LogDebug("Executing POST@{name}.", nameof(ForgotPassword));
			_logger.LogDebug("POST@{name}: check if user is authenticated.", nameof(this.ResetPassword));

			if (this.CheckForAuthenticatedUser("~/", out string? redirectTo))
			{
				_logger.LogDebug("GET@{name}: user is authenticated, redirecting.", nameof(this.ResetPassword));
				return this.Redirect(redirectTo!);
			}

			_logger.LogDebug("POST@{name}: user is not authenticated.", nameof(this.ResetPassword));
			_logger.LogDebug("POST@{name}: validate command.", nameof(this.ResetPassword));
			ResetPasswordCommandValidator validator = new ResetPasswordCommandValidator();
			ValidationResult validationresult = validator.Validate(command);

			if (validationresult.IsValid)
			{
				_logger.LogDebug("POST@{name}: command is valid.", nameof(this.ResetPassword));
				_logger.LogDebug("POST@{name}: send command to handler.", nameof(this.ResetPassword));
				HandlerResult result = await _mediator.Send(command);

				if (result.Success)
				{
					return this.RedirectToAction("SignIn");
				}
				else
				{
					_logger.LogDebug("POST@{name}: reset password failed.", nameof(this.ResetPassword));
					result.AddToModelState(this.ModelState);
				}
			}
			else
			{
				_logger.LogDebug("POST@{name}: command is not valid.", nameof(this.ResetPassword));
				validationresult.AddToModelState(this.ModelState);
			}

			_logger.LogDebug("Executed POST@{name}.", nameof(this.ResetPassword));
			return this.View(command);
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
