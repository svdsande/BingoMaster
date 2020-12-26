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
		[InlineData("MediumPassword", PasswordStrength.Medium)]
		[InlineData("Str0ngPassw0rd", PasswordStrength.Strong)]
		[InlineData("$tr0ngPassw0rd!", PasswordStrength.VeryStrong)]
		public void GetPasswordStrength_Succeeds(string password, PasswordStrength strength)
		{
			var actual = _passwordLogic.GetPasswordStrength(password);

			Assert.Equal(strength, actual);
		}
	}
}
