using BingoMaster_Logic;
using BingoMaster_Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BingoMaster_Tests
{
	public class PasswordLogicTests
	{
		private readonly IPasswordLogic _passwordLogic;

		public PasswordLogicTests()
		{
			_passwordLogic = new PasswordLogic();
		}

		[Theory]
		[InlineData("", PasswordStrength.Blank)]
		[InlineData("    ", PasswordStrength.Blank)]
		[InlineData("bad", PasswordStrength.VeryWeak)]
		[InlineData("password", PasswordStrength.Weak)]
		[InlineData("mediumpassword", PasswordStrength.Medium)]
		[InlineData("8529637410852", PasswordStrength.Strong)]
		[InlineData("StrongPassword", PasswordStrength.Strong)]
		[InlineData("$trongPassword!", PasswordStrength.VeryStrong)]
		public void GetPasswordStrength_Succeeds(string password, PasswordStrength strength)
		{
			var actual = _passwordLogic.GetPasswordStrength(password);

			Assert.Equal(strength, actual);
		}
	}
}
