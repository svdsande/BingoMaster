using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic.Interfaces
{
	public interface IPasswordLogic
	{
		string GetHashedPassword(string password, byte[] salt);
		bool VerifyPassword(string password, string hash, string salt);
		PasswordStrength GetPasswordStrength(string password);
		byte[] GetRandomSalt();
	}
}
