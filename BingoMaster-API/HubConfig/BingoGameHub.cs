using BingoMaster_Logic.Interfaces;
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

		public async Task GetNextNumber()
		{
			await Clients.All.SendAsync("NextNumber", _bingoGameLogic.GetNextNumber());
		}
	}
}
