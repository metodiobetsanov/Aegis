namespace Aegis.UnitTests.Core.Services.DataProtection
{
	using global::Aegis.Core.Services.DataProtection;
	using global::Aegis.Models.Settings;

	public class AegisLookupProtectorTests
	{
		private readonly byte[] _key = new byte[] { 211, 153, 132, 234, 131, 167, 136, 212, 195, 8, 128, 117, 135, 246, 211, 171, 123, 80, 197, 235, 151, 10, 222, 98, 55, 12, 198, 78, 193, 225, 161, 244 };
		private readonly string _keyId = "9d6d5e154ffe4e52bbdbbd737999ac75";
		private readonly Mock<ILookupProtectorKeyRing> _kr = new Mock<ILookupProtectorKeyRing>();
		private readonly IdentityProviderSettings _settings = new IdentityProviderSettings
		{
			LookupProtectorEncryptionDerivationPassword = nameof(AegisLookupProtectorTests),
			LookupProtectorSigningDerivationPassword = nameof(AegisLookupProtectorTests)
		};

		public AegisLookupProtectorTests()
		{
			_kr.Setup(x => x.CurrentKeyId).Returns(_keyId);
			_kr.Setup(x => x[It.Is<string>(k => k == _keyId)]).Returns(Convert.ToBase64String(_key));
		}

		[Theory]
		[InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit.")]
		[InlineData("Sed molestie urna quis volutpat tristique. Cras dapibus convallis mi, ac fermentum massa hendrerit eget.")]
		[InlineData("Praesent sed lectus libero. Aliquam ut molestie dolor.")]
		[InlineData("Ut dapibus, arcu at vehicula porta, odio nunc sollicitudin nunc, vel lacinia sapien lectus in lacus.")]
		[InlineData("Curabitur non massa tortor. Praesent ut dolor non orci vulputate cursus. Pellentesque euismod urna sed aliquam dapibus.")]

		public void AegisLookupProtectorShouldProtectUnprotect(string dataToProtect)
		{
			// Arrange
			AegisLookupProtector p = new AegisLookupProtector(_settings, _kr.Object);

			// Act
			string? protectedData = p.Protect(_keyId, dataToProtect);
			string? unProtectedData = p.Unprotect(_keyId, protectedData);

			// Assert
			protectedData.ShouldNotBeNullOrWhiteSpace();
			unProtectedData.ShouldNotBeNullOrWhiteSpace();
			unProtectedData.ShouldBe(dataToProtect);
		}

		[Fact]
		public void UnprotectShouldTrowErrorOnSignatureCheck()
		{
			// Arrange
			string data = "CoQCZmB2ctD3g3LEaLFiGO6B44JKrihJyOO6woTNDPRBqHlWic3iyeKO0l2taGZTRMAhjNSPyVd+GnF7xbTPgXmDMUMQIcD2PGzlk+qk7RXirIJcyvt2Lxto5aHZb7lDtoM+XYQdz4i8qYrDx+0XlFrxzI9+HcgnE2A2nST1wn1xIrD8Y6V20tOryFHH7bWsj4Q7qWAhjPIwPKCbxsiImTbUom1TNclU+2ZMVAriU4YtDNVvMdaOC6XYYw9ApwkJlwPai6QHrLXNn7AhdSBFOg==";
			AegisLookupProtector p = new AegisLookupProtector(_settings, _kr.Object);

			// Act
			Exception? exception = Record.Exception(() => p.Unprotect(_keyId, data));

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<CryptographicException>();
			exception.Message.ShouldBe("Invalid Signature.");
		}
	}
}
