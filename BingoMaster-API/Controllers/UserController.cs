using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BingoMaster_API.Attributes;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using BingoMaster_Models.User;
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
		[SwaggerResponse(HttpStatusCode.OK, typeof(AuthenticatedUserModel))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
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

		[HttpPost("register")]
		[SwaggerResponse(HttpStatusCode.OK, typeof(UserModel))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public IActionResult Register([FromBody] RegisterUserModel registerUserModel)
		{
			if (registerUserModel == null)
			{
				return BadRequest("Invalid register model provided");
			}

			var userModel = _userLogic.Register(registerUserModel);

			return Ok(userModel);
		}

		[HttpGet]
		[Authorize]
		[SwaggerResponse(HttpStatusCode.OK, typeof(UserModel))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public IActionResult GetUser(Guid id)
		{
			if (id == Guid.Empty)
			{
				return BadRequest("No user id provided");
			}

			var userModel = _userLogic.GetUserById(id);

			return Ok(userModel);
		}

		[HttpGet("email-unique")]
		[SwaggerResponse(HttpStatusCode.OK, typeof(bool))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public IActionResult EmailAddressUnique(string emailAddress)
		{
			if (string.IsNullOrWhiteSpace(emailAddress))
			{
				return BadRequest("No email address provided");
			}

			return Ok(_userLogic.EmailAddressUnique(emailAddress));
		}

		[HttpPut]
		[Authorize]
		[SwaggerResponse(HttpStatusCode.NoContent, typeof(void))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public IActionResult UpdateUser([FromBody] UserModel userModel)
		{
			if (userModel == null)
			{
				return BadRequest("Invalid form data provided");
			}

			_userLogic.Update(userModel);

			return NoContent();
		}
	}
}
