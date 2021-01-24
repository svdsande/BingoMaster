using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
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

		[HttpPost]
		[SwaggerResponse(HttpStatusCode.OK, typeof(BingoGameModel))]
		[SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
		public ActionResult CreateBingoGame([FromBody] BingoGameDetailModel gameCreationModel)
		{
			var bingoGameModel = _bingoGameLogic.CreateNewGame(gameCreationModel);

			return Ok(bingoGameModel);
		}
	}
}
