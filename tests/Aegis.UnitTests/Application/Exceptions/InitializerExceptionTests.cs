namespace Aegis.UnitTests.Application.Exceptions
{
	using global::Aegis.Application.Constants;
	using global::Aegis.Application.Exceptions;

	public class InitializerExceptionTests
	{
		public static readonly string DefaultMessage = InitializerConstants.SomethingWentWrong;
		public static readonly string TestMessage = nameof(TestMessage);
		public static readonly string TestDebugMessage = nameof(TestDebugMessage);

		[Fact]
		public void Test_Constructor_One()
		{
			/// Act
			InitializerException ex = new InitializerException();

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
			InitializerException ex = new InitializerException(TestMessage);

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
			InitializerException ex = new InitializerException(TestMessage, TestDebugMessage);

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
			InitializerException ex = new InitializerException(TestMessage, inner);

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
			InitializerException ex = new InitializerException(TestMessage, TestDebugMessage, inner);

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
			InitializerException ex = new InitializerException(TestMessage, TestDebugMessage);

			string exAsString = JsonConvert.SerializeObject(ex);
			InitializerException newEx = JsonConvert.DeserializeObject<InitializerException>(exAsString)!;

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
