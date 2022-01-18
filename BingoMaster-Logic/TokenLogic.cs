using BingoMaster_Entities;
using BingoMaster_Logic.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BingoMaster_Logic
{
    public class TokenLogic : ITokenLogic
    {
		private readonly string _key;

		public TokenLogic(IConfiguration configuration)
        {
			_key = configuration["JwtSecret"];
		}

		public string GenerateToken(User user)
        {
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_key);
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
    }
}
