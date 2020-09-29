using BingoMaster_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic.Interfaces
{
	public interface IBingoGameLogic
	{
		IEnumerable<BingoCardModel> CreateNewGame(string name, int amountOfPlayers, int size);
		int GetNextNumber();
	}
}
