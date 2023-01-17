namespace Aegis.Application.Initializers
{
	using System.Text;

	using Aegis.Application.Constants;
	using Aegis.Application.Contracts.Application;
	using Aegis.Application.Events.Audit.IdentityProvider;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Helpers;
	using Aegis.Persistence.Entities.IdentityProvider;

	using MediatR;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	using Newtonsoft.Json;

	/// <summary>
	/// Identity Initializer
	/// </summary>
	/// <seealso cref="Aegis.Services.Abstractions.IServiceInitializer" />
	public sealed class IdentityInitializer : IInitializer
	{
		/// <summary>
		/// The logger
		/// </summary>
		private readonly ILogger<IdentityInitializer> _logger;

		/// <summary>
		/// The scope factory
		/// </summary>
		private readonly IServiceScopeFactory _scopeFactory;

		/// <summary>
		/// Gets a value indicating whether this <see cref="IInitializer" /> is initialized.
		/// </summary>
		/// <value>
		///   <c>true</c> if initialized; otherwise, <c>false</c>.
		/// </value>
		public bool Initialized { get; private set; } = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="IdentityInitializer"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <param name="scopeFactory">The scope factory.</param>
		public IdentityInitializer(ILogger<IdentityInitializer> logger, IServiceScopeFactory scopeFactory)
		{
			_logger = logger;
			_scopeFactory = scopeFactory;
		}

		/// <summary>
		/// Starting the initializing of the service.
		/// </summary>
		public async Task Initialize()
		{
			if (!this.Initialized)
			{
				try
				{
					using (IServiceScope scope = _scopeFactory.CreateScope())
					{
						_logger.LogInformation("Identity Initializer: Setting up {ApplicationName} Initial Identity.", ApplicationConstants.ApplicationName);
						IMediator mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
						RoleManager<AegisRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AegisRole>>();
						UserManager<AegisUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AegisUser>>();

						this.Initialized = await this.InitializeRole(
							roleManager,
							mediator,
							IdentityProviderConstants.RootRole,
							"Aegis root server role.",
							true,
							true,
							false);

						this.Initialized = this.Initialized && await this.InitializeRole(
							roleManager,
							mediator,
							IdentityProviderConstants.OperatorRole,
							"Aegis base Operator role.",
							true,
							true,
							false);

						this.Initialized = this.Initialized && await this.InitializeRole(
							roleManager,
							mediator,
							IdentityProviderConstants.UserRole,
							"Aegis base User role.",
							true,
							true,
							true);

						this.Initialized = this.Initialized && await this.InitializeUser(userManager, mediator, IdentityProviderConstants.RootUserName);

						_logger.LogInformation("{ApplicationName} Identity Initializer was successful: {Initialized}.", ApplicationConstants.ApplicationName, this.Initialized);
					}
				}
				catch (Exception ex) when (ex is not InitializerException)
				{
					this.Initialized = false;
					this._logger.LogError(ex, "Identity Initializer Error: {message}", ex.Message);

					throw new InitializerException(
						InitializerConstants.SomethingWentWrong,
						$"Identity Initializer Error: {ex.Message}", ex);
				}
			}
		}

		/// <summary>
		/// Initializes the user.
		/// </summary>
		/// <param name="userManager">The user manager.</param>
		/// <param name="mediator">The mediator.</param>
		/// <param name="userName">Name of the user.</param>
		/// <exception cref="Aegis.Application.Exceptions.InitializerException">
		/// Identity Initializer: failed to create user.
		/// or
		/// Identity Initializer: failed to add roles to the user.
		/// </exception>
		private async Task<bool> InitializeUser(UserManager<AegisUser> userManager, IMediator mediator, string userName)
		{
			_logger.LogInformation("Identity Initializer: check if user exists.");
			bool result = true;
			AegisUser? identityUser = await userManager.FindByNameAsync(userName);

			if (identityUser is null)
			{
				try
				{
					_logger.LogInformation("Identity Initializer: attempting to create the root user.");
					identityUser = new AegisUser()
					{
						FirstName = "John",
						LastName = "Doe",
						UserName = userName,
						NormalizedUserName = userName.ToUpper(),
						Email = userName,
						NormalizedEmail = userName.ToUpper(),
						EmailConfirmed = true,
						SystemUser = true,
						ProtectedUser = true,
						ActiveProfile = true,
						CompletedProfile = true
					};

					string password = IdentityProviderHelper.GenerateRandomPassword();

					_logger.LogInformation("Identity Initializer: creating the root user.");
					IdentityResult userResult = await userManager.CreateAsync(identityUser, password);

					if (userResult.Succeeded)
					{
						_logger.LogInformation("Identity Initializer: user created.");
						string encPass = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
						_logger.LogInformation("LRUP: {encPass}", encPass);
						await mediator.Publish(new CreateRoleSucceededAuditEvent(
						identityUser.Id, "Identity Initializer: created the root user.", true, null, JsonConvert.SerializeObject(identityUser)));
					}
					else
					{
						result = false;
						List<string> errors = new List<string>();

						foreach (IdentityError? error in userResult.Errors)
						{
							_logger.LogError("Identity Initializer Error: {Code} => {Description}", error.Code, error.Description);
							errors.Add(error.Description);
						}

						await mediator.Publish(new CreateUserFailedAuditEvent(
							identityUser.Id, $"Identity Initializer: failed to create the root user. {string.Join(", ", errors)}", true, null, JsonConvert.SerializeObject(identityUser)));
					}
				}
				catch (Exception ex)
				{
					this.Initialized = false;
					this._logger.LogError(ex, "Identity Initializer Error: {message}", ex.Message);

					await mediator.Publish(new CreateUserFailedAuditEvent(
							identityUser!.Id, "Identity Initializer: failed to the create user. Error: " + ex.Message, true, null, JsonConvert.SerializeObject(identityUser)));

					throw new InitializerException(
						InitializerConstants.SomethingWentWrong,
						$"Identity Initializer Error: {ex.Message}", ex);
				}
			}

			if (result)
			{
				_logger.LogInformation("Identity Initializer: user created, adding roles.");
				result = await this.AddRolesToUser(userManager, mediator, identityUser);
			}

			return result;
		}

		/// <summary>
		/// Adds the roles to user.
		/// </summary>
		/// <param name="userManager">The user manager.</param>
		/// <param name="mediator">The mediator.</param>
		/// <param name="identityUser">The identity user.</param>
		/// <returns></returns>
		private async Task<bool> AddRolesToUser(UserManager<AegisUser> userManager, IMediator mediator, AegisUser identityUser)
		{
			bool result = true;

			try
			{
				_logger.LogInformation("Identity Initializer: check if user has roles.");
				List<string> roles = new List<string> { IdentityProviderConstants.RootRole, IdentityProviderConstants.OperatorRole, IdentityProviderConstants.UserRole };
				List<string> rolesToAdd = new List<string>();
				IList<string>? userRoles = await userManager.GetRolesAsync(identityUser);

				if (userRoles is not null)
				{
					rolesToAdd.AddRange(roles.Where(r => !userRoles.Contains(r)));
				}
				else
				{
					rolesToAdd.AddRange(roles);
				}

				if (rolesToAdd.Count > 0)
				{
					IdentityResult addToRolesResult = await userManager.AddToRolesAsync(identityUser, rolesToAdd);

					if (addToRolesResult.Succeeded)
					{
						_logger.LogInformation("Identity Initializer: roles assigned.");
						await mediator.Publish(new AssignRoleSucceededAuditEvent(
						identityUser.Id, "Identity Initializer: add roles to the root user.", true, null, string.Join(",", rolesToAdd)));
					}
					else
					{
						result = false;
						List<string> errors = new List<string>();

						foreach (IdentityError? error in addToRolesResult.Errors)
						{
							_logger.LogError("Identity Initializer Error: {Code} => {Description}", error.Code, error.Description);
							errors.Add(error.Description);
						}

						await mediator.Publish(new AssignRoleFailedAuditEvent(
							identityUser.Id, $"Identity Initializer: add roles to the root user. Error: {string.Join(", ", errors)}", true, null, string.Join(",", rolesToAdd)));
					}
				}
			}
			catch (Exception ex)
			{
				this.Initialized = false;
				this._logger.LogError(ex, "Identity Initializer Error: {message}", ex.Message);
				await mediator.Publish(new AssignRoleFailedAuditEvent(
						identityUser.Id, "Identity Initializer: add roles to the root user. Error: " + ex.Message, true, null, JsonConvert.SerializeObject(identityUser)));

				throw new InitializerException(
					InitializerConstants.SomethingWentWrong,
					$"Identity Initializer Error: {ex.Message}", ex);
			}

			return result;
		}

		/// <summary>
		/// Initializes the role.
		/// </summary>
		/// <param name="roleManager">The role manager.</param>
		/// <param name="mediator">The mediator.</param>
		/// <param name="roleName">Name of the role.</param>
		/// <param name="description">The description.</param>
		/// <param name="protectedRole">if set to <c>true</c> [protected role].</param>
		/// <param name="systemRole">if set to <c>true</c> [system role].</param>
		/// <param name="assignByDefault">if set to <c>true</c> [assign by default].</param>
		/// <exception cref="Aegis.Application.Exceptions.InitializerException">Failed to create '{roleName}' role.</exception>
		private async Task<bool> InitializeRole(RoleManager<AegisRole> roleManager, IMediator mediator, string roleName, string description, bool protectedRole, bool systemRole, bool assignByDefault)
		{
			_logger.LogInformation("Identity Initializer: check if role '{roleName}' exists.", roleName);
			bool result = true;
			AegisRole? role = await roleManager.FindByNameAsync(roleName);

			if (role is null)
			{
				try
				{
					_logger.LogInformation("Identity Initializer: attempting to create '{roleName}' role.", roleName);
					role = new AegisRole(roleName, description, protectedRole, systemRole, assignByDefault);
					IdentityResult roleResult = await roleManager.CreateAsync(role);

					if (roleResult.Succeeded)
					{
						_logger.LogInformation("Identity Initializer: role '{roleName}' created.", roleName);
						await mediator.Publish(new CreateRoleSucceededAuditEvent(
							role.Id, $"Identity Initializer: add role '{roleName}'.", true, null, JsonConvert.SerializeObject(role)));
					}
					else
					{
						result = false;
						List<string> errors = new List<string>();

						foreach (IdentityError? error in roleResult.Errors)
						{
							_logger.LogError("Identity Initializer Error:  {Code} => {Description}", error.Code, error.Description);
							errors.Add(error.Description);
						}

						await mediator.Publish(new CreateRoleFailedAuditEvent(
							role.Id, $"Identity Initializer: add role '{roleName}'. Error: {string.Join(", ", errors)}", true, null, JsonConvert.SerializeObject(role)));
					}
				}
				catch (Exception ex)
				{
					this.Initialized = false;
					this._logger.LogError(ex, "Identity Initializer Error: {message}", ex.Message);
					await mediator.Publish(new CreateRoleFailedAuditEvent(
							role!.Id, "Identity Initializer: add role '{roleName}'. Error: " + ex.Message, true, null, JsonConvert.SerializeObject(role)));

					throw new InitializerException(
						InitializerConstants.SomethingWentWrong,
						$"Identity Initializer Error: {ex.Message}", ex);
				}
			}

			return result;
		}
	}
}
