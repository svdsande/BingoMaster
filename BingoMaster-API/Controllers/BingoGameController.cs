using BingoMaster_API.Attributes;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Net;

namespace BingoMaster_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BingoGameController : ControllerBase
	{
		#region Fields

		private readonly IBingoGameLogic _bingoGameLogic;

		#endregion

		public BingoGameController(IBingoGameLogic bingoGameLogic)
		{
			_bingoGameLogic = bingoGameLogic;
		}

		[HttpGet]
		[SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<BingoGameDetailModel>))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public ActionResult GetAllBingoGames()
		{
			var games = _bingoGameLogic.GetAllPublicBingoGames();

			return Ok(games);
		}

		[HttpPost]
		[Authorize]
		[SwaggerResponse(HttpStatusCode.OK, typeof(BingoGameModel))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public ActionResult CreateBingoGame([FromBody] BingoGameDetailModel gameCreationModel)
		{
			if (gameCreationModel == null)
			{
				return BadRequest("Invalid game creation model provided");
			}

			var bingoGameModel = _bingoGameLogic.CreateNewGame(gameCreationModel);

			return Ok(bingoGameModel);
		}

		[HttpPut("{id}/players/join")]
		[Authorize]
		[SwaggerResponse(HttpStatusCode.OK, typeof(void))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public ActionResult JoinGame(Guid id, Guid playerId)
		{
			if (id == Guid.Empty || playerId == Guid.Empty)
			{
				return BadRequest("No game or player id provided");
			}

			_bingoGameLogic.JoinGame(id, playerId);

			return NoContent();
		}

		[HttpPut("{id}/players/leave")]
		[Authorize]
		[SwaggerResponse(HttpStatusCode.OK, typeof(void))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public ActionResult LeaveGame(Guid id, Guid playerId)
		{
			if (id == Guid.Empty || playerId == Guid.Empty)
			{
				return BadRequest("No game or player id provided");
			}

			_bingoGameLogic.LeaveGame(id, playerId);

			return NoContent();
		}
	}
}
