using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BingoMaster_API.Helpers
{
	public class JwtMiddleware
	{
		#region Fields

		private readonly RequestDelegate _next;
		private readonly JwtSettingsModel _jwtSettings;

		#endregion

		public JwtMiddleware(RequestDelegate next, IOptions<JwtSettingsModel> jwtSettings)
		{
			_next = next;
			_jwtSettings = jwtSettings.Value;
		}

		public async Task Invoke(HttpContext context, IUserLogic userLogic)
		{
			var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			if (token != null)
			{
                AttachUserToContext(context, userLogic, token);
			}

			await _next(context);
		}

        private void AttachUserToContext(HttpContext context, IUserLogic userLogic, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                context.Items["User"] = userLogic.GetUserByIdWithPlayer(userId);
            }
            catch
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
