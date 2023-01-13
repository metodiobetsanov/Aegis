namespace Aegis.UnitTests.Application.Services.DataProtection
{
	using global::Aegis.Application.Services.DataProtection;

	public class AegisInternalLookupProtectorKeyRingTests
	{
		[Fact]
		public void KeysShouldBeTheSame()
		{
			// Arrange
			AegisInternalLookupProtectorKeyRing kr1 = new AegisInternalLookupProtectorKeyRing();
			AegisInternalLookupProtectorKeyRing kr2 = new AegisInternalLookupProtectorKeyRing();

			IEnumerable<string> kr1List = kr1.GetAllKeyIds();
			IEnumerable<string> kr2List = kr2.GetAllKeyIds();

			// Act
			IEnumerable<string> result = kr1List.Where(kh => !kr2List.Contains(kh));

			// Assert
			result.Count().ShouldBe(0);
		}

		[Fact]
		public void CurrentKeyShouldNotBeNull()
		{
			for (int i = 0; i < 100; i++)
			{
				AegisInternalLookupProtectorKeyRing kr = new AegisInternalLookupProtectorKeyRing();
				string? result = kr[kr.CurrentKeyId];

				result.ShouldNotBeNullOrWhiteSpace();
			}
		}
	}
}
