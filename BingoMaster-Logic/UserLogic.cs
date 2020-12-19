﻿using BingoMaster_Entities;
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
				FirstName = user.FirstName,
				LastName = user.LastName
			};
		}

		public void Register(RegisterUserModel registerUserModel)
		{
			if (registerUserModel == null || string.IsNullOrWhiteSpace(registerUserModel.EmailAddress) || string.IsNullOrWhiteSpace(registerUserModel.Password))
			{
				throw new ArgumentException("No email address or password provided");
			}

			//TODO: Check password length and strength

			var user = _context.Users.FirstOrDefault(user => user.EmailAddress == registerUserModel.EmailAddress);

			if (user != null)
			{
				throw new ArgumentException("Email address is already taken");
			}

			var salt = _passwordLogic.GetRandomSalt();
			var hashedPassword = _passwordLogic.GetHashedPassword(registerUserModel.Password, salt);

			var newUser = new User
			{
				EmailAddress = registerUserModel.EmailAddress,
				FirstName = registerUserModel.FirstName,
				LastName = registerUserModel.LastName,
				Salt = Convert.ToBase64String(salt),
				Hash = hashedPassword
			};

			_context.Add(newUser);
			_context.SaveChanges();
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
