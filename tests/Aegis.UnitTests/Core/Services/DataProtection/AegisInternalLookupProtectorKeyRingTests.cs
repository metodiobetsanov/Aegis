namespace Aegis.UnitTests.Core.Services.DataProtection
{
	using global::Aegis.Core.Exceptions;
	using global::Aegis.Core.Services.DataProtection;
	using global::Aegis.Persistence.Contracts;
	using global::Aegis.Persistence.Entities.Application;

	using MediatR;

	using Microsoft.Extensions.DependencyInjection;

	public class AegisInternalLookupProtectorKeyRingTests
	{
		private readonly string _key = "OWQ2ZDVlMTU0ZmZlNGU1MmJiZGJiZDczNzk5OWFjNzU=";
		private readonly Mock<IServiceScopeFactory> _scf;
		private readonly Mock<IMediator> _m = new Mock<IMediator>();
		private readonly Mock<IAegisContext> _ac = new Mock<IAegisContext>();

		public AegisInternalLookupProtectorKeyRingTests()
		{
			ServiceCollection sc = new ServiceCollection();
			sc.AddScoped<IAegisContext>(x => _ac.Object);
			sc.AddScoped<IMediator>(x => _m.Object);

			_scf = Helper.GetServiceScopeFactoryMock(sc);
		}

		[Fact]
		public void KeysShouldBeTheSame()
		{
			// Arrange
			_ac.Setup(x => x.PersonalDataProtectionKeys.GetEntities())
				.Returns(new List<PersonalDataProtectionKey> {
					new PersonalDataProtectionKey
					{
						Id= Guid.Parse("18b29352-359f-4d8e-9a20-765b46403904"),
						KeyHash = _key.GetHashCode().ToString(),
						Key = _key,
						ExpiresOn = DateTime.Now.AddDays(7)
					}
				});

			AegisLookupProtectorKeyRing kr1 = new AegisLookupProtectorKeyRing(_scf.Object);
			AegisLookupProtectorKeyRing kr2 = new AegisLookupProtectorKeyRing(_scf.Object);

			IEnumerable<string> kr1List = kr1.GetAllKeyIds();
			IEnumerable<string> kr2List = kr2.GetAllKeyIds();

			// Act
			IEnumerable<string> result = kr1List.Where(kh => !kr2List.Contains(kh));

			// Assert
			result.Count().ShouldBe(0);
		}

		[Fact]
		public void KeysShouldBeCorrect()
		{
			// Arrange
			_ac.Setup(x => x.PersonalDataProtectionKeys.GetEntities())
				.Returns(new List<PersonalDataProtectionKey> {
					new PersonalDataProtectionKey
					{
						Id= Guid.Parse("18b29352-359f-4d8e-9a20-765b46403904"),
						KeyHash = _key.GetHashCode().ToString(),
						Key = _key,
						ExpiresOn = DateTime.Now.AddDays(7)
					}
				});

			AegisLookupProtectorKeyRing kr = new AegisLookupProtectorKeyRing(_scf.Object);

			// Act
			string key = kr[_key.GetHashCode().ToString()];

			// Assert
			key.ShouldNotBeNull();
			key.ShouldBe(_key);
		}

		[Fact]
		public void ShouldReturnShortLivedKey()
		{
			// Arrange
			_ac.Setup(x => x.PersonalDataProtectionKeys.GetEntities())
				.Returns(new List<PersonalDataProtectionKey> {
						new PersonalDataProtectionKey
						{
							Id= Guid.Parse("18b29352-359f-4d8e-9a20-765b46403904"),
							KeyHash = _key.GetHashCode().ToString(),
							Key = _key,
							ExpiresOn = DateTime.Now.AddDays(1)
						}
					});

			_ac.Setup(x => x.SaveChanges()).Returns(1);

			AegisLookupProtectorKeyRing kr = new AegisLookupProtectorKeyRing(_scf.Object);

			// Act
			IEnumerable<string> krList = kr.GetAllKeyIds();

			// Assert
			krList.Count().ShouldBe(2);
		}

		[Fact]
		public void ShouldThrowAnExceptionFailedToSaveShortLivedKey()
		{
			// Arrange
			_ac.Setup(x => x.PersonalDataProtectionKeys.GetEntities())
				.Returns(new List<PersonalDataProtectionKey> {
						new PersonalDataProtectionKey
						{
							Id= Guid.Parse("18b29352-359f-4d8e-9a20-765b46403904"),
							KeyHash = _key.GetHashCode().ToString(),
							Key = _key,
							ExpiresOn = DateTime.Now.AddDays(1)
						}
					});

			_ac.Setup(x => x.SaveChanges()).Returns(0);

			// Act
			Exception exception = Record.Exception(() => new AegisLookupProtectorKeyRing(_scf.Object));

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<ServiceException>();
		}

		[Fact]
		public void ShouldThrowAnExceptionNoActiveKeys()
		{
			// Arrange
			_ac.Setup(x => x.PersonalDataProtectionKeys.GetEntities())
				.Returns(new List<PersonalDataProtectionKey> {
						new PersonalDataProtectionKey
						{
							Id= Guid.Parse("18b29352-359f-4d8e-9a20-765b46403904"),
							KeyHash = _key.GetHashCode().ToString(),
							Key = _key,
							ExpiresOn = DateTime.Now.AddDays(1)
						}
					});
			_ac.Setup(x => x.SaveChanges())
				.Throws(new OperationCanceledException());

			// Act
			Exception exception = Record.Exception(() => new AegisLookupProtectorKeyRing(_scf.Object));

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<ServiceException>();
			((ServiceException)exception).InnerException.ShouldNotBeNull();
			((ServiceException)exception).InnerException.ShouldBeOfType<OperationCanceledException>();
		}

		[Fact]
		public void CurrentKeyShouldNotBeNull()
		{
			// Arrange
			_ac.Setup(x => x.PersonalDataProtectionKeys.GetEntities())
				.Returns(new List<PersonalDataProtectionKey> {
					new PersonalDataProtectionKey
					{
						Id= Guid.Parse("18b29352-359f-4d8e-9a20-765b46403904"),
						KeyHash = _key.GetHashCode().ToString(),
						Key = _key,
						ExpiresOn = DateTime.Now.AddDays(7)
					}
				});

			AegisLookupProtectorKeyRing kr = new AegisLookupProtectorKeyRing(_scf.Object);

			// Act
			string? result = kr[kr.CurrentKeyId];

			// Assert
			result.ShouldNotBeNullOrWhiteSpace();
		}
	}
}
