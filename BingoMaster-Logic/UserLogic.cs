using BingoMaster_Entities;
using BingoMaster_Logic.Exceptions;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using BingoMaster_Models.User;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BingoMaster_Logic
{
	public class UserLogic : IUserLogic
	{
		#region Fields

		private readonly JwtSettingsModel _jwtSettings;
		private readonly BingoMasterDbContext _context;
		private readonly IPasswordLogic _passwordLogic;

		#endregion

		public UserLogic(IOptions<JwtSettingsModel> jwtSettings, BingoMasterDbContext context, IPasswordLogic passwordLogic)
		{
			_jwtSettings = jwtSettings.Value;
			_context = context;
			_passwordLogic = passwordLogic;
		}

		public AuthenticatedUserModel Authenticate(AuthenticateUserModel authenticateUserModel)
		{
			if (authenticateUserModel == null || string.IsNullOrWhiteSpace(authenticateUserModel.EmailAddress) || string.IsNullOrWhiteSpace(authenticateUserModel.Password))
			{
				throw new ArgumentException("No email address or password provided");
			}

			var user = _context.Users.FirstOrDefault(user => user.EmailAddress == authenticateUserModel.EmailAddress);

			if (user == null)
			{
				return null;
			}

			if (!_passwordLogic.VerifyPassword(authenticateUserModel.Password, user.Hash, user.Salt))
			{
				throw new Exception("Login failed");
			}

			return new AuthenticatedUserModel
			{
				Id = user.Id,
				EmailAddress = authenticateUserModel.EmailAddress,
				FirstName = user.FirstName,
				LastName = user.LastName,
				UserName = user.UserName,
				Token = GenerateToken(user)
			};
		}

		public UserModel GetUserById(Guid id)
		{
			var user = _context.Users.Find(id);

			return new UserModel
			{
				Id = user.Id,
				EmailAddress = user.EmailAddress,
				UserName = user.UserName,
				FirstName = user.FirstName,
				LastName = user.LastName
			};
		}

		public UserModel Register(RegisterUserModel registerUserModel)
		{
			if (registerUserModel == null || string.IsNullOrWhiteSpace(registerUserModel.EmailAddress) || string.IsNullOrWhiteSpace(registerUserModel.Password) || string.IsNullOrWhiteSpace(registerUserModel.UserName))
			{
				throw new ArgumentException("No email address, username or password provided");
			}

			if (!UserNameUnique(registerUserModel.UserName) || !EmailAddressUnique(registerUserModel.EmailAddress))
			{
				throw new UserAlreadyExistsException("User already exists");
			}

			var passwordStrength = _passwordLogic.GetPasswordStrength(registerUserModel.Password);

			if (passwordStrength != PasswordStrength.Strong && passwordStrength != PasswordStrength.VeryStrong)
			{
				throw new ArgumentException("Provided password too weak");
			}

			var salt = _passwordLogic.GetRandomSalt();
			var hashedPassword = _passwordLogic.GetHashedPassword(registerUserModel.Password, salt);

			var newUser = new User
			{
				EmailAddress = registerUserModel.EmailAddress,
				UserName = registerUserModel.UserName,
				FirstName = registerUserModel.FirstName,
				LastName = registerUserModel.LastName,
				Salt = Convert.ToBase64String(salt),
				Hash = hashedPassword
			};

			_context.Add(newUser);
			_context.SaveChanges();

			return new UserModel
			{
				Id = newUser.Id,
				EmailAddress = newUser.EmailAddress,
				UserName = newUser.UserName,
				FirstName = newUser.FirstName,
				LastName = newUser.LastName
			};
		}

		public bool UserNameUnique(string userName)
		{
			if (string.IsNullOrWhiteSpace(userName))
			{
				throw new ArgumentException("No username provided");
			}

			var user = _context.Users.FirstOrDefault(user => user.UserName == userName);

			if (user != null)
			{
				return false;
			}

			return true;
		}

		public bool EmailAddressUnique(string emailAddress)
		{
			if (string.IsNullOrWhiteSpace(emailAddress))
			{
				throw new ArgumentException("No email address provided");
			}

			var user = _context.Users.FirstOrDefault(user => user.EmailAddress == emailAddress);

			if (user != null)
			{
				return false;
			}

			return true;
		}

		private string GenerateToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
