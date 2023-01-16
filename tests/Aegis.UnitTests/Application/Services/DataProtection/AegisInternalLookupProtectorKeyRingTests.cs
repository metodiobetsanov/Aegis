namespace Aegis.UnitTests.Application.Services.DataProtection
{
	using global::Aegis.Application.Exceptions;
	using global::Aegis.Application.Services.DataProtection;
	using global::Aegis.Persistence.Contracts;
	using global::Aegis.Persistence.Entities.Application;

	using MediatR;

	using Microsoft.Extensions.DependencyInjection;

	public class AegisInternalLookupProtectorKeyRingTests
	{
		private readonly string _key = "OWQ2ZDVlMTU0ZmZlNGU1MmJiZGJiZDczNzk5OWFjNzU=";
		private readonly Mock<IMediator> _m = new Mock<IMediator>();
		private readonly Mock<IServiceScopeFactory> _scf = new Mock<IServiceScopeFactory>();
		private readonly Mock<IServiceScope> _sc = new Mock<IServiceScope>();
		private readonly Mock<IAegisContext> _ac = new Mock<IAegisContext>();

		public AegisInternalLookupProtectorKeyRingTests()
		{
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
			ServiceCollection sc = new ServiceCollection();
			sc.AddScoped<IAegisContext>(x => _ac.Object);
			sc.AddScoped<IMediator>(x => _m.Object);

			_sc.Setup(x => x.ServiceProvider)
				.Returns(sc.BuildServiceProvider());

			_scf.Setup(x => x.CreateScope())
				.Returns(_sc.Object);
		}

		[Fact]
		public void KeysShouldBeTheSame()
		{
			// Arrange
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
			for (int i = 0; i < 100; i++)
			{
				AegisLookupProtectorKeyRing kr = new AegisLookupProtectorKeyRing(_scf.Object);
				string? result = kr[kr.CurrentKeyId];

				result.ShouldNotBeNullOrWhiteSpace();
			}
		}
	}
}
