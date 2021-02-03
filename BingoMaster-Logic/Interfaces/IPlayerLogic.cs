using BingoMaster_Models;
using BingoMaster_Models.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic.Interfaces
{
	public interface IPlayerLogic
	{
		IEnumerable<BingoGameDetailModel> GetGamesForPlayer(Guid id);
		PlayerModel GetPlayerById(Guid id);
		IEnumerable<PlayerModel> GetAllPlayers();
		void Update(PlayerModel playerModel);
		bool PlayerNameUnique(string playerName);
	}
}
