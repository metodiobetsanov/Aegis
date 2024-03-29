﻿#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

namespace Aegis.Core.Commands.Authentication.Handlers
{
	using System.Threading;
	using System.Threading.Tasks;

	using Aegis.Core.Commands.Authentication;
	using Aegis.Core.Constants;
	using Aegis.Core.Contracts;
	using Aegis.Core.Contracts.CQRS;
	using Aegis.Core.Events.Audit.IdentityProvider;
	using Aegis.Core.Exceptions;
	using Aegis.Core.Helpers;
	using Aegis.Models.Authentication;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Castle.Core.Logging;

	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using MediatR;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.Logging;

	using Newtonsoft.Json;

	/// <summary>
	/// SignUp Command Handler
	/// </summary>
	/// <seealso cref="Aegis.Core.Contracts.CQRS.ICommandHandler&lt;Aegis.Core.Commands.Authentication.SignUpCommand, Aegis.Models.Authentication.SignUpCommandResult&gt;" />
	public sealed class SignUpCommandHandler : ICommandHandler<SignUpCommand, SignUpCommandResult>
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<SignUpCommandHandler> _logger;

		/// <summary>
		/// The mediator
		/// </summary>
		private readonly IMediator _mediator;

		/// <summary>
		/// The interaction
		/// </summary>
		private readonly IIdentityServerInteractionService _interaction;

		/// <summary>
		/// The user manager.
		/// </summary>
		private readonly UserManager<AegisUser> _userManager;

		/// <summary>
		/// The role manager
		/// </summary>
		private readonly RoleManager<AegisRole> _roleManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="SignUpCommandHandler"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="mediator">The mediator.</param>
		/// <param name="interaction">The interaction.</param>
		/// <param name="userManager">The user manager.</param>
		/// <param name="roleManager">The role manager.</param>
		public SignUpCommandHandler(
			ILogger<SignUpCommandHandler> logger,
			IMediator mediator,
			IIdentityServerInteractionService interaction,
			UserManager<AegisUser> userManager,
			RoleManager<AegisRole> roleManager)
		{
			_logger = logger;
			_mediator = mediator;
			_interaction = interaction;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		/// <summary>
		/// Handles the specified command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns>
		///  <see cref="SignUpCommandResult" />
		/// </returns>
		/// <exception cref="Aegis.Core.Exceptions.IdentityProviderException"></exception>
		public async Task<SignUpCommandResult> Handle(SignUpCommand command, CancellationToken cancellationToken)
		{
			_logger.LogDebug("Handling {name}", nameof(SignUpCommand));
			SignUpCommandResult signUpCommandResult = SignUpCommandResult.Failed();

			try
			{

				_logger.LogDebug("SignUpCommandHandler: get authorization context and validate return URL.");
				AuthorizationRequest? context = await _interaction.GetAuthorizationContextAsync(command.ReturnUrl);
				string returnUrl = context.GetReturnUrl(command.ReturnUrl!);

				_logger.LogDebug("SignUpCommandHandler: check if email/ exists.");
				AegisUser? user = await _userManager.FindByEmailAsync(command.Email!);

				if (user is not null)
				{
					_logger.LogDebug("SignUpCommandHandler: email/username exists.");
					signUpCommandResult.Errors.Add(new KeyValuePair<string, string>("Email", "This email address is already registered!"));
				}
				else
				{
					_logger.LogDebug("SignUpCommandHandler: create new user.");
					user = new AegisUser(command.Email!);
					IdentityResult userResult = await _userManager.CreateAsync(user, command.Password!);

					if (userResult.Succeeded)
					{
						_logger.LogDebug("SignUpCommandHandler: user created.");
						await _mediator.Publish(new CreateUserSucceededAuditEvent(
								user.Id,
								"Created user.",
								null,
								JsonConvert.SerializeObject(user)),
							cancellationToken);

						signUpCommandResult = await this.AddRolesToUserAsync(user, returnUrl, cancellationToken);
					}
					else
					{
						_logger.LogDebug("SignUpCommandHandler: failed create user.");
						await _mediator.Publish(
							 new CreateUserFailedAuditEvent(
									user.Id,
									"Failed to create user.",
									null,
									JsonConvert.SerializeObject(userResult.Errors)),
						cancellationToken);

						userResult.AddToFailedResult(signUpCommandResult);
					}
				}
			}
			catch (Exception ex) when (ex is not IAegisException)
			{
				_logger.LogError(ex, "SignUpCommandHandler Error: {Message}", ex.Message);
				throw new IdentityProviderException(IdentityProviderConstants.SomethingWentWrongWithSignUp, ex.Message, ex);
			}

			_logger.LogDebug("Handling {name}", nameof(SignUpCommand));
			return signUpCommandResult;
		}

		/// <summary>
		/// Adds the roles to user.
		/// </summary>
		/// <param name="user">The user.</param>
		/// <param name="returnUrl">The return URL.</param>
		/// <param name="cancellationToken">The cancellation token.</param>
		/// <returns></returns>
		private async Task<SignUpCommandResult> AddRolesToUserAsync(AegisUser user, string returnUrl, CancellationToken cancellationToken)
		{
			SignUpCommandResult signUpCommandResult = SignUpCommandResult.Failed();

			_logger.LogDebug("SignUpCommandHandler: get all default roles and assign them to the user.");
			IEnumerable<string> roles = _roleManager.Roles
				.Where(r => r.AssignByDefault)
				.Select(r => r.Name ?? string.Empty)
				.ToList();

			if (roles.Any())
			{
				IdentityResult rolesResult = await _userManager.AddToRolesAsync(user, roles);

				if (rolesResult.Succeeded)
				{
					_logger.LogDebug("SignUpCommandHandler: roles assigned to user.");
					await _mediator.Publish(new AssignRoleSucceededAuditEvent(
							user.Id,
							"Assign roles to user.",
							null,
							string.Join(",", roles)),
						cancellationToken);

					signUpCommandResult = SignUpCommandResult.Succeeded(user.Id.ToString(), returnUrl);
				}
				else
				{
					_logger.LogDebug("SignUpCommandHandler: failed to assign roles to user.");
					await _mediator.Publish(new AssignRoleFailedAuditEvent(
							user.Id,
							"Failed to assign roles to user.",
							null,
							JsonConvert.SerializeObject(rolesResult.Errors)),
						cancellationToken);

					rolesResult.AddToFailedResult(signUpCommandResult);
				}
			}
			else
			{
				signUpCommandResult = SignUpCommandResult.Succeeded(user.Id.ToString(), returnUrl);
			}

			return signUpCommandResult;
		}
	}
}
