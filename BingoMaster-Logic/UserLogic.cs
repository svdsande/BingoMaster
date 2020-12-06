using BingoMaster_Entities;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BingoMaster_Logic
{
	public class UserLogic : IUserLogic
	{
		#region Fields

		private readonly JwtSettingsModel _jwtSettings;
		private readonly BingoMasterDbContext _context;

		#endregion

		public UserLogic(IOptions<JwtSettingsModel> jwtSettings, BingoMasterDbContext context)
		{
			_jwtSettings = jwtSettings.Value;
			_context = context;
		}

		public AuthenticatedUserModel Authenticate(AuthenticateUserModel authenticateUserModel)
		{
			if (authenticateUserModel == null || string.IsNullOrWhiteSpace(authenticateUserModel.EmailAddress) || string.IsNullOrWhiteSpace(authenticateUserModel.Password))
			{
				throw new ArgumentException("No email address or password provided");
			}

			var user = _context.Users.Find(authenticateUserModel.EmailAddress, authenticateUserModel.Password);

			if (user == null)
			{
				return null;
			}

			var token = GenerateToken();

			return new AuthenticatedUserModel
			{
				EmailAddress = authenticateUserModel.EmailAddress,
				Token = token
			};
		}

		private string GenerateToken()
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("id", "Foo") }),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
