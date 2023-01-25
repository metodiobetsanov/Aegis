namespace Aegis.UnitTests.Application.Queries.Authentication.Handlers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	using Duende.IdentityServer.Models;
	using Duende.IdentityServer.Services;
	using Duende.IdentityServer.Validation;

	using global::Aegis.Application.Constants;
	using global::Aegis.Application.Constants.Services;
	using global::Aegis.Application.Exceptions;
	using global::Aegis.Application.Queries.Authentication;
	using global::Aegis.Application.Queries.Authentication.Handlers;
	using global::Aegis.Models.Authentication;
	using global::Aegis.Persistence.Entities.IdentityProvider;

	using Moq;

	public class GetUserLockedTimeQueryHandlerTests
	{
		private static readonly Faker _faker = new Faker("en");
		private static readonly Faker<AegisUser> _fakeUser = Helper.GetUserFaker();

		private readonly Mock<ILogger<GetUserLockedTimeQueryHandler>> _logger = new Mock<ILogger<GetUserLockedTimeQueryHandler>>();
		private readonly Mock<UserManager<AegisUser>> _userManager = Helper.GetUserManagerMock();

		[Fact]
		public void Handle_ShouldReturnTrue_OnValidUser()
		{
			// Arrange
			AegisUser user = _fakeUser.Generate();

			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync(user);
			_userManager.Setup(x => x.GetLockoutEndDateAsync(It.IsAny<AegisUser>()))
				.ReturnsAsync(DateTime.UtcNow.AddMinutes(10));

			GetUserLockedTimeQuery query = new GetUserLockedTimeQuery { UserId = _faker.Random.Guid().ToString() };
			GetUserLockedTimeQueryHandler handler = new GetUserLockedTimeQueryHandler(_logger.Object, _userManager.Object);

			// Act 
			GetUserLockedTimeQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeTrue();
			result.LockedTill.ShouldNotBeNull();
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnEmptyUserID()
		{
			// Arrange
			GetUserLockedTimeQuery query = new GetUserLockedTimeQuery();
			GetUserLockedTimeQueryHandler handler = new GetUserLockedTimeQueryHandler(_logger.Object, _userManager.Object);

			// Act 
			GetUserLockedTimeQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.Errors.Count.ShouldBe(1);
		}

		[Fact]
		public void Handle_ShouldReturnFalse_OnNonExistingUser()
		{
			// Arrange
			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync((AegisUser?)null);

			GetUserLockedTimeQuery query = new GetUserLockedTimeQuery { UserId = _faker.Random.Guid().ToString() };
			GetUserLockedTimeQueryHandler handler = new GetUserLockedTimeQueryHandler(_logger.Object, _userManager.Object);

			// Act 
			GetUserLockedTimeQueryResult result = handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult();

			// Assert
			result.ShouldNotBeNull();
			result.Success.ShouldBeFalse();
			result.Errors.Count.ShouldBe(1);
		}

		[Fact]
		public void Handle_Exceptions()
		{
			// Arrange
			_userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
				.Throws(new Exception(nameof(Exception)));

			GetUserLockedTimeQuery query = new GetUserLockedTimeQuery { UserId = _faker.Random.Guid().ToString() };
			GetUserLockedTimeQueryHandler handler = new GetUserLockedTimeQueryHandler(_logger.Object, _userManager.Object);

			// Act 
			Exception exception = Record.Exception(() => handler.Handle(query, new CancellationToken()).GetAwaiter().GetResult());

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<IdentityProviderException>();
			((IdentityProviderException)exception).Message.ShouldBe(IdentityProviderConstants.SomethingWentWrongWithSignIn);
			((IdentityProviderException)exception).InnerException.ShouldNotBeNull();
			((IdentityProviderException)exception).InnerException!.Message.ShouldBe(nameof(Exception));
		}
	}
}
