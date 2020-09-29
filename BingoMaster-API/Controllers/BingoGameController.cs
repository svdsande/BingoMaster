using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;

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
		[SwaggerResponse(System.Net.HttpStatusCode.OK, typeof(IEnumerable<BingoCardModel>))]
		[SwaggerResponse(System.Net.HttpStatusCode.BadRequest, typeof(string))]
		public ActionResult CreateBingoGame(string name, int amountOfPlayers, int size)
		{
			var bingoCards = _bingoGameLogic.CreateNewGame(name, amountOfPlayers, size);

			return Ok(bingoCards);
		}
	}
}
