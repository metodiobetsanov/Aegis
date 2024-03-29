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
	using global::Aegis.Persistence.Contracts;
	using global::Aegis.Persistence.Entities.Application;

	using MediatR;

	public class EncryptionInitializerTests
	{
		private static readonly Faker _faker = new Faker("en");

		private readonly Mock<IServiceScopeFactory> _scf;
		private readonly Mock<ILogger<EncryptionInitializer>> _logger = new Mock<ILogger<EncryptionInitializer>>();
		private readonly Mock<IRepository<PersonalDataProtectionKey>> _pdpkRepo = new Mock<IRepository<PersonalDataProtectionKey>>();
		private readonly Mock<IAegisContext> _ac = new Mock<IAegisContext>();

		public EncryptionInitializerTests()
		{
			_ac.Setup(x => x.PersonalDataProtectionKeys)
				.Returns(_pdpkRepo.Object);

			ServiceCollection services = Helper.GetServiceCollection();
			services.AddScoped<IAegisContext>(x => _ac.Object);
			services.AddScoped<IMediator>(x => new Mock<IMediator>().Object);
			_scf = Helper.GetServiceScopeFactoryMock(services);
		}

		[Fact]
		public void Initialize_ShouldBeTrue()
		{
			// Arrange
			_pdpkRepo.Setup(x => x.GetEntities()).Returns(new List<PersonalDataProtectionKey>());
			_ac.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(1));

			EncryptionInitializer initializer = new EncryptionInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			initializer.Initialize().GetAwaiter().GetResult();

			// Assert
			initializer.Initialized.ShouldBeTrue();
		}

		[Fact]
		public void Initialize_ShouldBeTrue_OnExistingKeys()
		{
			// Arrange
			_pdpkRepo.Setup(x => x.GetEntities()).Returns(new List<PersonalDataProtectionKey> {
				new PersonalDataProtectionKey { Id = Guid.NewGuid(), Key = _faker.Random.String2(12), KeyHash = _faker.Random.String2(36), ExpiresOn = DateTime.UtcNow, } });

			EncryptionInitializer initializer = new EncryptionInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			initializer.Initialize().GetAwaiter().GetResult();

			// Assert
			initializer.Initialized.ShouldBeTrue();
		}

		[Fact]
		public void Initialize_ShouldBeFalse_OnFailedSave()
		{
			// Arrange
			_pdpkRepo.Setup(x => x.GetEntities()).Returns(new List<PersonalDataProtectionKey>());
			_ac.Setup(x => x.SaveChangesAsync()).Returns(Task.FromResult(0));

			EncryptionInitializer initializer = new EncryptionInitializer(_logger.Object, _scf.Object);
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

			EncryptionInitializer initializer = new EncryptionInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			Exception exception = Record.Exception(() => initializer.Initialize().GetAwaiter().GetResult());

			// Assert
			initializer.Initialized.ShouldBeFalse();
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<InitializerException>();
		}

		[Fact]
		public void Initialize_ShouldBeFalse_OnSaveChangesException()
		{
			// Arrange
			_pdpkRepo.Setup(x => x.GetEntities()).Returns(new List<PersonalDataProtectionKey>());
			_ac.Setup(x => x.SaveChangesAsync()).Throws(new Exception());

			EncryptionInitializer initializer = new EncryptionInitializer(_logger.Object, _scf.Object);
			initializer.Initialized.ShouldBeFalse();

			// Act
			Exception exception = Record.Exception(() => initializer.Initialize().GetAwaiter().GetResult());

			// Assert
			initializer.Initialized.ShouldBeFalse();
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<InitializerException>();
		}
	}
}
