using BingoMaster_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic.Interfaces
{
	public interface IBingoGameLogic
	{
		BingoGameModel CreateNewGame(BingoGameCreationModel gameCreationModel);
		BingoGameModel PlayRound(IEnumerable<PlayerModel> players, int[] drawnNumbers);
	}
}
