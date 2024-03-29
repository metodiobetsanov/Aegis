﻿#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Core.Initializers
{
	using global::Aegis.Core.Exceptions;
	using global::Aegis.Core.Initializers;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	using MediatR;

	public class IdentityInitializerTests
	{
		private static readonly Faker _faker = new Faker("en");

		private readonly Mock<IServiceScopeFactory> _scf;
		private readonly Mock<ILogger<IdentityInitializer>> _logger = new Mock<ILogger<IdentityInitializer>>();
		private readonly Mock<UserManager<AegisUser>> _userManager = Helper.GetUserManagerMock();
		private readonly Mock<RoleManager<AegisRole>> _roleManager = Helper.GetRoleManagerMock();

		public IdentityInitializerTests()
		{
			ServiceCollection services = Helper.GetServiceCollection();
			services.AddScoped<IMediator>(x => new Mock<IMediator>().Object);
			services.AddScoped<UserManager<AegisUser>>(x => _userManager.Object);
			services.AddScoped<RoleManager<AegisRole>>(x => _roleManager.Object);
			_scf = Helper.GetServiceScopeFactoryMock(services);
		}

		[Fact]
		public void Initialize_ShouldBeTrue()
		{
			// Arrange
			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<List<string>>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_roleManager.Setup(x => x.CreateAsync(It.IsAny<AegisRole>()))
				.Returns(Task.FromResult(IdentityResult.Success));

			IdentityInitializer initializer = new IdentityInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			initializer.Initialize().GetAwaiter().GetResult();

			// Assert
			initializer.Initialized.ShouldBeTrue();
		}

		[Fact]
		public void Initialize_ShouldBeTrue_ExistingRoles()
		{
			// Arrange
			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_userManager.Setup(x => x.GetRolesAsync(It.IsAny<AegisUser>()))
				.Returns(Task.FromResult(new List<string> { "testRole" } as IList<string>));
			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<List<string>>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_roleManager.Setup(x => x.CreateAsync(It.IsAny<AegisRole>()))
				.Returns(Task.FromResult(IdentityResult.Success));

			IdentityInitializer initializer = new IdentityInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			initializer.Initialize().GetAwaiter().GetResult();

			// Assert
			initializer.Initialized.ShouldBeTrue();
		}

		[Fact]
		public void Initialize_ShouldBeFalse_OnFailedToAddUser()
		{
			// Arrange
			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.Returns(Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "Code", Description = "Error" })));
			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<List<string>>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_roleManager.Setup(x => x.CreateAsync(It.IsAny<AegisRole>()))
				.Returns(Task.FromResult(IdentityResult.Success));

			IdentityInitializer initializer = new IdentityInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			initializer.Initialize().GetAwaiter().GetResult();

			// Assert
			initializer.Initialized.ShouldBeFalse();
		}

		[Fact]
		public void Initialize_ShouldBeFalse_OnFailedToAddRole()
		{
			// Arrange
			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<List<string>>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_roleManager.Setup(x => x.CreateAsync(It.IsAny<AegisRole>()))
				.Returns(Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "Code", Description = "Error" })));

			IdentityInitializer initializer = new IdentityInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			initializer.Initialize().GetAwaiter().GetResult();

			// Assert
			initializer.Initialized.ShouldBeFalse();
		}

		[Fact]
		public void Initialize_ShouldBeFalse_OnFailedToAddRolesToUsers()
		{
			// Arrange
			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<List<string>>()))
				.Returns(Task.FromResult(IdentityResult.Failed(new IdentityError { Code = "Code", Description = "Error" })));
			_roleManager.Setup(x => x.CreateAsync(It.IsAny<AegisRole>()))
				.Returns(Task.FromResult(IdentityResult.Success));

			IdentityInitializer initializer = new IdentityInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			initializer.Initialize().GetAwaiter().GetResult();

			// Assert
			initializer.Initialized.ShouldBeFalse();
		}

		[Fact]
		public void Initialize_ShouldBeFalse_OnException()
		{
			// Arrange
			_scf.Setup(x => x.CreateScope()).Throws(new Exception());

			IdentityInitializer initializer = new IdentityInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			Exception exception = Record.Exception(() => initializer.Initialize().GetAwaiter().GetResult());

			// Assert
			initializer.Initialized.ShouldBeFalse();
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<InitializerException>();
		}

		[Fact]
		public void Initialize_ShouldBeFalse_OnCreateUserException()
		{
			// Arrange
			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.Throws(new Exception());
			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<List<string>>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_roleManager.Setup(x => x.CreateAsync(It.IsAny<AegisRole>()))
				.Returns(Task.FromResult(IdentityResult.Success));

			IdentityInitializer initializer = new IdentityInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			Exception exception = Record.Exception(() => initializer.Initialize().GetAwaiter().GetResult());

			// Assert
			initializer.Initialized.ShouldBeFalse();
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<InitializerException>();
		}

		[Fact]
		public void Initialize_ShouldBeFalse_OnCreateRoleException()
		{
			// Arrange
			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<List<string>>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_roleManager.Setup(x => x.CreateAsync(It.IsAny<AegisRole>()))
				.Throws(new Exception());

			IdentityInitializer initializer = new IdentityInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			Exception exception = Record.Exception(() => initializer.Initialize().GetAwaiter().GetResult());

			// Assert
			initializer.Initialized.ShouldBeFalse();
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<InitializerException>();
		}

		[Fact]
		public void Initialize_ShouldBeFalse_OnAddUserToRolesException()
		{
			// Arrange
			_userManager.Setup(x => x.CreateAsync(It.IsAny<AegisUser>(), It.IsAny<string>()))
				.Returns(Task.FromResult(IdentityResult.Success));
			_userManager.Setup(x => x.AddToRolesAsync(It.IsAny<AegisUser>(), It.IsAny<List<string>>()))
				.Throws(new Exception());
			_roleManager.Setup(x => x.CreateAsync(It.IsAny<AegisRole>()))
				.Returns(Task.FromResult(IdentityResult.Success));

			IdentityInitializer initializer = new IdentityInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			Exception exception = Record.Exception(() => initializer.Initialize().GetAwaiter().GetResult());

			// Assert
			initializer.Initialized.ShouldBeFalse();
			exception.ShouldNotBeNull();
		}
	}
}
