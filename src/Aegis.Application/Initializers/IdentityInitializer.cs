namespace Aegis.Application.Initializers
{
	using System.Text;

	using Aegis.Application.Constants;
	using Aegis.Application.Contracts.Application;
	using Aegis.Application.Exceptions;
	using Aegis.Application.Helpers;
	using Aegis.Persistence.Entities.IdentityProvider;

	using Microsoft.AspNetCore.Identity;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	/// <summary>
	/// Service Initializer
	/// </summary>
	/// <seealso cref="Chimera.Services.Abstractions.IServiceInitializer" />
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
		/// The initialized
		/// </summary>
		private bool _initialized = false;

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
			if (!_initialized)
			{
				try
				{
					using (IServiceScope scope = _scopeFactory.CreateScope())
					{
						_logger.LogInformation("Service Initializer: Setting up {ApplicationName} Initial Identity.", ApplicationConstants.ApplicationName);
						RoleManager<AegisRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AegisRole>>();
						UserManager<AegisUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<AegisUser>>();

						await this.InitializeRole(
							roleManager,
							IdentityProviderConstants.RootRole,
							"Chimera root server role.",
							true,
							true,
							false);
						await this.InitializeRole(
							roleManager,
							IdentityProviderConstants.OperatorRole,
							"Chimera base Operator role.",
							true,
							true,
							false);
						await this.InitializeRole(
							roleManager,
							IdentityProviderConstants.UserRole,
							"Chimera base User role.",
							true,
							true,
							true);
						await this.InitializeUser(userManager, IdentityProviderConstants.RootUserName);
					}

					_logger.LogInformation("{ApplicationName} Initial Identity is Initialized.", ApplicationConstants.ApplicationName);
					_initialized = true;
				}
				catch (Exception ex) when (ex is not InitializerException)
				{
					throw new InitializerException(
						"Identity Initializer Error!",
						$"Identity Initializer Error: {ex.Message}", ex);
				}
			}
		}

		/// <summary>
		/// Initializes the user.
		/// </summary>
		/// <param name="userManager">The user manager.</param>
		/// <param name="userName">Name of the user.</param>
		/// <exception cref="Chimera.Application.Exceptions.InitializerException">
		/// Service Initializer: failed to create user.
		/// or
		/// Service Initializer: failed to add roles to the user.
		/// </exception>
		private async Task InitializeUser(UserManager<AegisUser> userManager, string userName)
		{
			_logger.LogInformation("Service Initializer: check if user exists.");
			AegisUser? identityUser = await userManager.FindByNameAsync(userName);

			if (identityUser is null)
			{
				_logger.LogInformation("Service Initializer: attempting to create user.");
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

				_logger.LogInformation("Service Initializer: creating user.");
				IdentityResult userResult = await userManager.CreateAsync(identityUser, password);

				if (!userResult.Succeeded)
				{
					foreach (IdentityError? error in userResult.Errors)
					{
						_logger.LogError("Service Initializer Error:  {Code} => {Description}", error.Code, error.Description);
					}

					throw new InitializerException("Service Initializer: failed to create user.");
				}

				string encPass = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
				_logger.LogInformation("Service Initializer: user created.");
				_logger.LogInformation("LRUP: {encPass}", encPass);
			}

			_logger.LogInformation("Service Initializer: check if user has roles.");
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

				if (!addToRolesResult.Succeeded)
				{
					foreach (IdentityError? error in addToRolesResult.Errors)
					{
						_logger.LogError("Service Initializer Error:  {Code} => {Description}", error.Code, error.Description);
					}
				}

				_logger.LogInformation("Service Initializer: roles assigned.");
			}
		}

		/// <summary>
		/// Initializes the role.
		/// </summary>
		/// <param name="roleManager">The role manager.</param>
		/// <param name="roleName">Name of the role.</param>
		/// <param name="description">The description.</param>
		/// <param name="protectedRole">if set to <c>true</c> [protected role].</param>
		/// <param name="systemRole">if set to <c>true</c> [system role].</param>
		/// <param name="assignByDefault">if set to <c>true</c> [assign by default].</param>
		/// <exception cref="Chimera.Application.Exceptions.InitializerException">Service Initializer: failed to create a role.</exception>
		private async Task InitializeRole(RoleManager<AegisRole> roleManager, string roleName, string description, bool protectedRole, bool systemRole, bool assignByDefault)
		{
			_logger.LogInformation("Service Initializer: check if role '{roleName}' exists.", roleName);
			AegisRole? role = await roleManager.FindByNameAsync(roleName);

			if (role is null)
			{
				_logger.LogInformation("Identity Initializer: attempting to create '{roleName}' role.", roleName);

				role = new AegisRole(roleName, description, protectedRole, systemRole, assignByDefault);
				IdentityResult roleResult = await roleManager.CreateAsync(role);

				if (!roleResult.Succeeded)
				{
					foreach (IdentityError? error in roleResult.Errors)
					{
						_logger.LogError("Identity Initializer Error:  {Code} => {Description}", error.Code, error.Description);
					}

					throw new InitializerException($"Failed to create '{roleName}' role.");
				}

				_logger.LogInformation("Identity Initializer: role '{roleName}' created.", roleName);
			}
		}
	}
}
