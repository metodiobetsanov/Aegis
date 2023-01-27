#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Application.Exceptions
{
	using global::Aegis.Constants;
	using global::Aegis.Exceptions;

	public class ApplicationFlowExceptionTests
	{
		public static readonly string DefaultMessage = HostConstants.SomethingWentWrong;
		public static readonly string TestMessage = nameof(TestMessage);
		public static readonly string TestDebugMessage = nameof(TestDebugMessage);

		[Fact]
		public void Test_Constructor_One()
		{
			/// Act
			ApplicationFlowException ex = new ApplicationFlowException();

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
			ApplicationFlowException ex = new ApplicationFlowException(TestMessage);

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
			ApplicationFlowException ex = new ApplicationFlowException(TestMessage, TestDebugMessage);

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
			ApplicationFlowException ex = new ApplicationFlowException(TestMessage, inner);

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
			ApplicationFlowException ex = new ApplicationFlowException(TestMessage, TestDebugMessage, inner);

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
			ApplicationFlowException ex = new ApplicationFlowException(TestMessage, TestDebugMessage);

			string exAsString = JsonConvert.SerializeObject(ex);
			ApplicationFlowException newEx = JsonConvert.DeserializeObject<ApplicationFlowException>(exAsString)!;

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
