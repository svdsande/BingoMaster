using BingoMaster_Logic.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace BingoMaster_Logic
{
	public class PasswordLogic : IPasswordLogic
	{
		public PasswordLogic() { }

		public string GetHashedPassword(string password, byte[] salt)
		{
			return Convert.ToBase64String(KeyDerivation.Pbkdf2(
			password: password,
			salt: salt,
			prf: KeyDerivationPrf.HMACSHA1,
			iterationCount: 10000,
			numBytesRequested: 256 / 8));
		}

		public bool VerifyPassword(string password, string hash, string salt)
		{
			var saltBytes = Convert.FromBase64String(salt);
			var hashedPassword = GetHashedPassword(password, saltBytes);

			return hashedPassword == hash;
		}

		public PasswordStrength GetPasswordStrength(string password)
		{
			if (string.IsNullOrWhiteSpace(password))
			{
				return PasswordStrength.Blank;
			}

			if (password.Length < 4)
			{
				return PasswordStrength.VeryWeak;
			}
		}

		public byte[] GetRandomSalt()
		{
			byte[] salt = new byte[128 / 8];

			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(salt);

			return salt;
		}
	}

	public enum PasswordStrength
	{
		Blank,
		VeryWeak,
		Weak,
		Medium,
		Strong,
		VeryStrong
	}
}
