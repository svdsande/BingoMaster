using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using BingoMaster_Models.Player;
using Microsoft.AspNetCore.Authorization;
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

		[HttpGet("{id}")]
		[Authorize]
		[SwaggerResponse(HttpStatusCode.OK, typeof(PlayerModel))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public IActionResult GetPlayer(Guid id)
		{
			if (id == Guid.Empty)
			{
				return BadRequest("No player id provided");
			}

			return Ok(_playerLogic.GetPlayerById(id));
		}

		[HttpGet]
		[Authorize]
		[SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<PlayerModel>))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public IActionResult GetAllPlayers()
		{
			return Ok(_playerLogic.GetAllPlayers());
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

		[HttpGet("playername-unique")]
		[SwaggerResponse(HttpStatusCode.OK, typeof(bool))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public IActionResult PlayerNameUnique(string playerName)
		{
			if (string.IsNullOrWhiteSpace(playerName))
			{
				return BadRequest("No player name provided");
			}

			return Ok(_playerLogic.PlayerNameUnique(playerName));
		}

		[HttpPut]
		[Authorize]
		[SwaggerResponse(HttpStatusCode.NoContent, typeof(void))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public IActionResult UpdatePlayer([FromBody] PlayerModel playerModel)
		{
			if (playerModel == null)
			{
				return BadRequest("Invalid form data provided");
			}

			_playerLogic.Update(playerModel);

			return NoContent();
		}
	}
}
