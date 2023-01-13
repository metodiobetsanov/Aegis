namespace Aegis.UnitTests.Application.Services.DataProtection
{
	using global::Aegis.Application.Constants;
	using global::Aegis.Application.Services.DataProtection;

	public class AegisPersonalDataProtectorTest
	{
		private readonly byte[] _key = new byte[] { 211, 153, 132, 234, 131, 167, 136, 212, 195, 8, 128, 117, 135, 246, 211, 171, 123, 80, 197, 235, 151, 10, 222, 98, 55, 12, 198, 78, 193, 225, 161, 244 };
		private readonly string _keyId = "9d6d5e154ffe4e52bbdbbd737999ac75";
		private readonly Mock<ILookupProtectorKeyRing> _kr = new Mock<ILookupProtectorKeyRing>();
		private readonly Mock<ILookupProtector> _p = new Mock<ILookupProtector>();

		public AegisPersonalDataProtectorTest()
		{
			_kr.Setup(x => x.CurrentKeyId).Returns(_keyId);
			_kr.Setup(x => x[It.Is<string>(k => k == _keyId)]).Returns(Convert.ToBase64String(_key));
			_p.Setup(x => x.Protect(It.IsAny<string>(), It.IsAny<string>()))
				.Returns((string key, string data) => data.ToBase64());
			_p.Setup(x => x.Unprotect(It.IsAny<string>(), It.IsAny<string>()))
				.Returns((string key, string data) => Encoding.UTF8.GetString(Convert.FromBase64String(data)));
		}

		[Theory]
		[InlineData("Lorem ipsum dolor sit amet, consectetur adipiscing elit.")]
		[InlineData("Sed molestie urna quis volutpat tristique. Cras dapibus convallis mi, ac fermentum massa hendrerit eget.")]
		[InlineData("Praesent sed lectus libero. Aliquam ut molestie dolor.")]
		[InlineData("Ut dapibus, arcu at vehicula porta, odio nunc sollicitudin nunc, vel lacinia sapien lectus in lacus.")]
		[InlineData("Curabitur non massa tortor. Praesent ut dolor non orci vulputate cursus. Pellentesque euismod urna sed aliquam dapibus.")]

		public void AegisPersonalDataProtectorShouldProtectUnprotect(string dataToProtect)
		{
			// Arrange
			AegisPersonalDataProtector p = new AegisPersonalDataProtector(_kr.Object, _p.Object);

			// Act
			string? protectedData = p.Protect(dataToProtect);
			string? unprotectedData = p.Unprotect(protectedData);

			// Assert
			protectedData.ShouldNotBeNullOrWhiteSpace();
			protectedData.ShouldBe(string.Format(IdentityProviderConstants.PersonalDataKeyDataStringFormat, _keyId, dataToProtect.ToBase64()));
			unprotectedData.ShouldNotBeNullOrWhiteSpace();
			unprotectedData.ShouldBe(dataToProtect);
		}

		[Fact]
		public void UnprotectShouldTrowErrorOnSignatureCheck()
		{
			// Arrange
			string data = "CoQCZmB2ctD3g3LEaLFiGO6B44JKrihJyOO6woTNDPRBqHlWic3iyeKO0l2taGZTRMAhjNSPyVd+GnF7xbTPgXmDMUMQIcD2PGzlk+qk7RXirIJcyvt2Lxto5aHZb7lDtoM+XYQdz4i8qYrDx+0XlFrxzI9+HcgnE2A2nST1wn1xIrD8Y6V20tOryFHH7bWsj4Q7qWAhjPIwPKCbxsiImTbUom1TNclU+2ZMVAriU4YtDNVvMdaOC6XYYw9ApwkJlwPai6QHrLXNn7AhdSBFOg==";
			AegisPersonalDataProtector p = new AegisPersonalDataProtector(_kr.Object, _p.Object);

			// Act
			Exception? exception = Record.Exception(() => p.Unprotect(data));

			// Assert
			exception.ShouldNotBeNull();
			exception.ShouldBeOfType<InvalidOperationException>();
			exception.Message.ShouldBe("Malformed data.");
		}
	}
}
