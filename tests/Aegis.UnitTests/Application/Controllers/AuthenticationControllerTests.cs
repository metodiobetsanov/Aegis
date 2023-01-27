namespace Aegis.UnitTests.Application.Controllers
{
	using global::Aegis.Controllers;

	public partial class AuthenticationControllerTests
	{
		#region Setup
		private static readonly Faker _faker = new Faker("en");

		private readonly Mock<ILogger<AuthenticationController>> _logger = new Mock<ILogger<AuthenticationController>>();
		private readonly Mock<IDataProtector> _dp = new Mock<IDataProtector>();
		private readonly Mock<IDataProtectionProvider> _dpp = new Mock<IDataProtectionProvider>();
		private readonly Mock<IMediator> _m = new Mock<IMediator>();
		private readonly Mock<ClaimsPrincipal> _cp = new Mock<ClaimsPrincipal>();
		private readonly Mock<HttpContext> _hc = new Mock<HttpContext>();

		public AuthenticationControllerTests()
		{
			_hc.Setup(x => x.User).Returns(_cp.Object);
			_dpp.Setup(x => x.CreateProtector(It.IsAny<string>())).Returns(_dp.Object);
		}
		#endregion Setup
	}
}
