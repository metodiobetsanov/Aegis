#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
namespace Aegis.UnitTests.Persistence.Exceptions
{
	using global::Aegis.Persistence.Constants;
	using global::Aegis.Persistence.Exceptions;

	public class PersistenceExceptionTests
	{
		public static readonly string DefaultMessage = PersistenceConstants.SomethingWentWrong;
		public static readonly string TestMessage = nameof(TestMessage);
		public static readonly string TestDebugMessage = nameof(TestDebugMessage);

		[Fact]
		public void Test_Constructor_One()
		{
			/// Act
			PersistenceException ex = new PersistenceException();

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
			PersistenceException ex = new PersistenceException(TestMessage);

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
			PersistenceException ex = new PersistenceException(TestMessage, TestDebugMessage);

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
			PersistenceException ex = new PersistenceException(TestMessage, inner);

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
			PersistenceException ex = new PersistenceException(TestMessage, TestDebugMessage, inner);

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
			PersistenceException ex = new PersistenceException(TestMessage, TestDebugMessage);

			string exAsString = JsonConvert.SerializeObject(ex);
			PersistenceException newEx = JsonConvert.DeserializeObject<PersistenceException>(exAsString)!;

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
