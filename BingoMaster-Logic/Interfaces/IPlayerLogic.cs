using BingoMaster_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic.Interfaces
{
	public interface IPlayerLogic
	{
		IEnumerable<BingoGameDetailModel> GetGamesForPlayer(Guid id);
	}
}
