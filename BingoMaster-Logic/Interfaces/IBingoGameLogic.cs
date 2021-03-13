using BingoMaster_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic.Interfaces
{
	public interface IBingoGameLogic
	{
		BingoGameModel CreateNewGame(BingoGameDetailModel gameCreationModel);
		BingoGameModel PlayRound(IEnumerable<PlayerGameModel> players, int[] drawnNumbers);
		IEnumerable<BingoGameDetailModel> GetAllPublicBingoGames();
		void JoinGame(Guid gameId, Guid playerId);
		void LeaveGame(Guid gameId, Guid playerId);
	}
}
