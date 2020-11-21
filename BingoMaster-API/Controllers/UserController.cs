using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace BingoMaster_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		#region Fields

		private readonly IUserLogic _userLogic;

		#endregion

		public UserController(IUserLogic userLogic)
		{
			_userLogic = userLogic;
		}

		[HttpPost("authenticate")]
		[SwaggerResponse(System.Net.HttpStatusCode.OK, typeof(AuthenticatedUserModel))]
		[SwaggerResponse(System.Net.HttpStatusCode.BadRequest, typeof(string))]
		public IActionResult Authenticate([FromBody] AuthenticateUserModel authenticateUserModel)
		{
			if (authenticateUserModel == null)
			{
				return BadRequest("Invalid authentication model provided");
			}

			var authenticatedUser = _userLogic.Authenticate(authenticateUserModel);

			if (authenticatedUser == null)
			{
				return BadRequest("Email address or password is incorrect");
			}

			return Ok(authenticatedUser);
		}
	}
}
