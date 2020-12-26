using BingoMaster_Logic.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

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

			var score = 1;

			if (password.Length >= 8)
			{
				score++;
			}

			// Contains numbers
			if (Regex.Match(password, @"[0-9]", RegexOptions.ECMAScript).Success)
			{
				score++;
			}

			// Both lowercase and uppercase
			if (Regex.Match(password, @"^(?=.*[a-z])(?=.*[A-Z]).+$", RegexOptions.ECMAScript).Success)
			{
				score++;
			}

			// Contains special character
			if (Regex.Match(password, @"[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success)
			{
				score++;
			}

			return (PasswordStrength)score;
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
