﻿using AutoMapper;
using BingoMaster_Entities;
using BingoMaster_Logic.Exceptions;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using BingoMaster_Models.User;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
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
		private readonly IMapper _mapper;

		#endregion

		public UserLogic(IOptions<JwtSettingsModel> jwtSettings, BingoMasterDbContext context, IPasswordLogic passwordLogic, IMapper mapper)
		{
			_jwtSettings = jwtSettings.Value;
			_context = context;
			_passwordLogic = passwordLogic;
			_mapper = mapper;
		}

		public AuthenticatedUserModel Authenticate(AuthenticateUserModel authenticateUserModel)
		{
			if (authenticateUserModel == null || string.IsNullOrWhiteSpace(authenticateUserModel.EmailAddress) || string.IsNullOrWhiteSpace(authenticateUserModel.Password))
			{
				throw new ArgumentException("No email address or password provided");
			}

			var user = _context.Users
				.Include(user => user.Player)
				.FirstOrDefault(user => user.EmailAddress == authenticateUserModel.EmailAddress);

			if (user == null)
			{
				return null;
			}

			if (!_passwordLogic.VerifyPassword(authenticateUserModel.Password, user.Hash, user.Salt))
			{
				throw new Exception("Login failed");
			}

			var model = _mapper.Map<AuthenticatedUserModel>(user);
			model.Token = GenerateToken(user);

			return model;
		}

		public UserModel GetUserByIdWithPlayer(Guid id)
		{
			var user = _context.Users
				.Include(user => user.Player)
				.SingleOrDefault(user => user.Id == id);

			return _mapper.Map<UserModel>(user);
		}

		public UserModel GetUserById(Guid id)
		{
			var user = _context.Users.Find(id);

			return _mapper.Map<UserModel>(user);
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
				Hash = hashedPassword,
				Player = new Player()
			};

			_context.Add(newUser);
			_context.SaveChanges();

			return _mapper.Map<UserModel>(newUser);
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
				Subject = new ClaimsIdentity(new[] { 
					new Claim("id", user.Id.ToString()),
					new Claim("playerId", user.Player.Id.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		public void Update(UserModel userModel)
		{
			var user = _context.Users.FirstOrDefault(user => user.Id == userModel.Id);

			if (user == null)
			{
				throw new KeyNotFoundException("Entity does not exists");
			}

			_context.Entry(user).CurrentValues.SetValues(userModel);
			_context.SaveChanges();
		}
	}
}
