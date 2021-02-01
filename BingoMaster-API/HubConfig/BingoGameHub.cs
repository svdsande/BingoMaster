using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BingoMaster_API.HubConfig
{
	public class BingoGameHub : Hub
	{
		#region Fields

		private readonly IBingoGameLogic _bingoGameLogic;

		#endregion

		public BingoGameHub(IBingoGameLogic bingoGameLogic)
		{
			_bingoGameLogic = bingoGameLogic;
		}

		public async Task PlayNextRound(IEnumerable<PlayerGameModel> players, int[] drawnNumbers)
		{
			await Clients.All.SendAsync("PlayNextRound", _bingoGameLogic.PlayRound(players, drawnNumbers));
		}
	}
}
