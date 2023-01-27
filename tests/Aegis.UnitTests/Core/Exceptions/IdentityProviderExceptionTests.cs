namespace Aegis.UnitTests.Core.Exceptions
{
	using global::Aegis.Core.Constants;
	using global::Aegis.Core.Exceptions;

	public class IdentityProviderExceptionTests
	{
		public static readonly string DefaultMessage = IdentityProviderConstants.SomethingWentWrong;
		public static readonly string TestMessage = nameof(TestMessage);
		public static readonly string TestDebugMessage = nameof(TestDebugMessage);

		[Fact]
		public void Test_Constructor_One()
		{
			/// Act
			IdentityProviderException ex = new IdentityProviderException();

			/// Assert
			ex.ShouldNotBeNull();
			ex.Message.ShouldNotBeNull();
			ex.Message.ShouldBe(DefaultMessage);
			ex.DebugMessage.ShouldNotBeNull();
			ex.DebugMessage.ShouldBe(DefaultMessage);
		}

		[Fact]
		public void Test_Constructor_Two()
		{
			/// Act
			IdentityProviderException ex = new IdentityProviderException(TestMessage);

			/// Assert
			ex.ShouldNotBeNull();
			ex.Message.ShouldNotBeNull();
			ex.Message.ShouldBe(TestMessage);
			ex.DebugMessage.ShouldNotBeNull();
			ex.DebugMessage.ShouldBe(TestMessage);
		}

		[Fact]
		public void Test_Constructor_Tree()
		{
			/// Act
			IdentityProviderException ex = new IdentityProviderException(TestMessage, TestDebugMessage);

			/// Assert
			ex.ShouldNotBeNull();
			ex.Message.ShouldNotBeNull();
			ex.Message.ShouldBe(TestMessage);
			ex.DebugMessage.ShouldNotBeNull();
			ex.DebugMessage.ShouldBe(TestDebugMessage);
		}

		[Fact]
		public void Test_Constructor_Four()
		{
			/// Arrange
			InvalidOperationException inner = new InvalidOperationException();

			/// Act
			IdentityProviderException ex = new IdentityProviderException(TestMessage, inner);

			/// Assert
			ex.ShouldNotBeNull();
			ex.Message.ShouldNotBeNull();
			ex.Message.ShouldBe(TestMessage);
			ex.DebugMessage.ShouldNotBeNull();
			ex.DebugMessage.ShouldBe(TestMessage);
			ex.InnerException.ShouldNotBeNull();
			ex.InnerException.ShouldBeOfType<InvalidOperationException>();
		}

		[Fact]
		public void Test_Constructor_Five()
		{
			/// Arrange
			InvalidOperationException inner = new InvalidOperationException();

			/// Act
			IdentityProviderException ex = new IdentityProviderException(TestMessage, TestDebugMessage, inner);

			/// Assert
			ex.ShouldNotBeNull();
			ex.Message.ShouldNotBeNull();
			ex.Message.ShouldBe(TestMessage);
			ex.DebugMessage.ShouldNotBeNull();
			ex.DebugMessage.ShouldBe(TestDebugMessage);
			ex.InnerException.ShouldNotBeNull();
			ex.InnerException.ShouldBeOfType<InvalidOperationException>();
		}

		[Fact]
		public void Test_Serialization()
		{
			/// Act
			IdentityProviderException ex = new IdentityProviderException(TestMessage, TestDebugMessage);

			string exAsString = JsonConvert.SerializeObject(ex);
			IdentityProviderException newEx = JsonConvert.DeserializeObject<IdentityProviderException>(exAsString)!;

			// Assert
			ex.ShouldNotBeNull();
			ex.ToString().Equals(newEx.ToString()).ShouldBeTrue();
			ex.Message.ShouldNotBeNull();
			ex.Message.Equals(newEx.Message).ShouldBeTrue();
			ex.DebugMessage.ShouldNotBeNull();
			ex.DebugMessage.Equals(newEx.DebugMessage).ShouldBeTrue();
		}
	}
}
