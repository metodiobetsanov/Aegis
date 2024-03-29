﻿#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Core.Commands.Authentication.Handlers
{
	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;

	using global::Aegis.Core.Constants;
	using global::Aegis.Core.Exceptions;
	using global::Aegis.Core.Queries.Authentication;
	using global::Aegis.Core.Queries.Authentication.Handlers;
	using global::Aegis.Models.Authentication;

	public class SignOutQueryHandlerTests
	{
		private static readonly Faker _faker = new Faker("en");

		private readonly Mock<ILogger<SignOutQueryHandler>> _logger = new Mock<ILogger<SignOutQueryHandler>>();
		private readonly Mock<IIdentityServerInteractionService> _isis = new Mock<IIdentityServerInteractionService>();

		[Fact]
		public void Handle_ShouldReturnTrue()
		{
			// Arrange
			string logoutId = _faker.Random.String2(12);
			_isis.Setup(x => x.GetLogoutContextAsync(It.Is<string>(s => s == logoutId)))
				.ReturnsAsync(new LogoutRequest(_faker.Internet.UrlRootedPath(), null));

			SignOutQuery query = new SignOutQuery { LogoutId = logoutId };
			SignOutQueryHandler handler = new SignOutQueryHandler(_logger.Object, _isis.Object);

			// Act 
			SignOutQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShowSignoutPrompt.ShouldBeTrue();
		}

		[Fact]
		public void Handle_ShouldReturnFalse()
		{
			// Arrange
			string logoutId = _faker.Random.String2(12);
			_isis.Setup(x => x.GetLogoutContextAsync(It.Is<string>(s => s == logoutId)))
				.ReturnsAsync(new LogoutRequest(_faker.Internet.UrlRootedPath(), new LogoutMessage { ClientId = _faker.Random.String2(12) }));

			SignOutQuery query = new SignOutQuery { LogoutId = logoutId };
			SignOutQueryHandler handler = new SignOutQueryHandler(_logger.Object, _isis.Object);

			// Act 
			SignOutQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.ShowSignoutPrompt.ShouldBeFalse();
		}

		[Fact]
		public void Handle_Exceptions()
		{
			// Arrange
			_isis.Setup(x => x.GetLogoutContextAsync(It.IsAny<string>()))
				.Throws(new Exception(nameof(Exception)));

			SignOutQuery query = new SignOutQuery();
			SignOutQueryHandler handler = new SignOutQueryHandler(_logger.Object, _isis.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<IdentityProviderException>();
			((IdentityProviderException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrongWithSignOut);
			((IdentityProviderException)exception).InnerException.ShouldNotBeNull();
			((IdentityProviderException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
