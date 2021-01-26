using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BingoMaster_API.Attributes;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace BingoMaster_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlayerController : ControllerBase
	{
		#region Fields

		private readonly IPlayerLogic _playerLogic;

		#endregion

		public PlayerController(IPlayerLogic playerLogic)
		{
			_playerLogic = playerLogic;
		}

		[HttpGet("{id}/games")]
		[Authorize]
		[SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<BingoGameDetailModel>))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public IActionResult GamesForPlayer(Guid id)
		{
			if (id == Guid.Empty)
			{
				return BadRequest("No player id provided");
			}

			return Ok(_playerLogic.GetGamesForPlayer(id));
		}
	}
}
