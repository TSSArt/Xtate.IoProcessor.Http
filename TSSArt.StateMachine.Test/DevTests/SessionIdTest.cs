﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TSSArt.StateMachine.Test.DevTests
{
	[TestClass]
	public class SessionIdTest
	{
		[TestMethod]
		public void SameNewIdsShouldBeEqualTest()
		{
			// arrange
			var s1 = SessionId.New();
			var s2 = s1;

			// act => assert
			AssertEquality(s1, s2);
		}

		[TestMethod]
		public void SameCustomIdsShouldBeEqualTest()
		{
			// arrange
			var s1 = SessionId.FromString("v" + 1);
			var s2 = SessionId.FromString("v" + 1);

			// act => assert
			AssertEquality(s1, s2);
		}

		[TestMethod]
		public void SameAutogeneratedIdsShouldBeEqualTest()
		{
			// arrange
			var s = SessionId.New();
			var s1 = SessionId.FromString(s.Value);
			var s2 = SessionId.FromString(s.Value);

			// act => assert
			AssertEquality(s1, s2);
		}

		[TestMethod]
		public void SameRestoredAndAutogeneratedIdsShouldBeEqualTest()
		{
			// arrange
			var s1 = SessionId.New();
			var s2 = SessionId.FromString(s1.Value);

			// act => assert
			AssertEquality(s1, s2);
		}

		[TestMethod]
		public void NewIdsShouldNotBeEqualTest()
		{
			// arrange
			var s1 = SessionId.New();
			var s2 = SessionId.New();

			// act => assert
			AssertInequality(s1, s2);
		}

		[SuppressMessage(category: "ReSharper", checkId: "ParameterOnlyUsedForPreconditionCheck.Local")]
		private static void AssertEquality(SessionId s1, SessionId s2)
		{
			Assert.IsTrue(s1.GetHashCode() == s2.GetHashCode());
			Assert.IsTrue(Equals(s1, s2));
			Assert.IsTrue(Equals(s2, s1));
			Assert.IsTrue(s1.Equals(s2));
			Assert.IsTrue(s2.Equals(s1));
			Assert.IsTrue(s2.Equals(s1));
			Assert.IsTrue(s1 == s2);
			Assert.IsTrue(s2 == s1);
			Assert.IsFalse(s1 != s2);
			Assert.IsFalse(s2 != s1);
		}

		[SuppressMessage(category: "ReSharper", checkId: "ParameterOnlyUsedForPreconditionCheck.Local")]
		private static void AssertInequality(SessionId s1, SessionId s2)
		{
			Assert.IsFalse(ReferenceEquals(s1, s2));
			Assert.IsFalse(Equals(s1, s2));
			Assert.IsFalse(Equals(s2, s1));
			Assert.IsFalse(s1.Equals(s2));
			Assert.IsFalse(s2.Equals(s1));
			Assert.IsFalse(s1 == s2);
			Assert.IsFalse(s2 == s1);
			Assert.IsTrue(s1 != s2);
			Assert.IsTrue(s2 != s1);
		}
	}
}