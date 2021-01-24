using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoMaster_Logic
{
	public class BingoGameLogic : IBingoGameLogic
	{
		#region Fields

		private readonly IBingoCardLogic _bingoCardLogic;
		private readonly IBingoNumberLogic _bingoNumberLogic;

		#endregion

		public BingoGameLogic(IBingoCardLogic bingoCardLogic, IBingoNumberLogic bingoNumberLogic)
		{
			_bingoCardLogic = bingoCardLogic;
			_bingoNumberLogic = bingoNumberLogic;
		}

		public BingoGameModel CreateNewGame(BingoGameDetailModel gameCreationModel)
		{
			if (gameCreationModel == null || string.IsNullOrWhiteSpace(gameCreationModel.Name) || gameCreationModel.Players.Count() <= 0 || gameCreationModel.Size <= 0)
			{
				throw new ArgumentException("No name for the game provided or invalid number of players or grid size");
			}

			foreach (var player in gameCreationModel.Players)
			{
				player.BingoCard = _bingoCardLogic.GenerateBingoCard(new BingoCardCreationModel()
				{
					Name = player.Name,
					IsCenterSquareFree = true,
					Size = gameCreationModel.Size
				});
			}

			return new BingoGameModel()
			{
				DrawnNumber = _bingoNumberLogic.GetNextNumber(),
				Players = gameCreationModel.Players
			};
		}

		public BingoGameModel PlayRound(IEnumerable<PlayerModel> players, int[] drawnNumbers)
		{
			if (players.Count() <= 0 || drawnNumbers.Length <= 0)
			{
				throw new ArgumentException("Invalid number of players or drawn numbers");
			}

			var nextNumber = _bingoNumberLogic.GetNextNumber();
			var numbers = drawnNumbers.Select(number => (int?)number).ToList();
			numbers.Add(nextNumber);

			foreach (var player in players)
			{
				player.IsHorizontalLineDone = HorizontalLineDone(player.BingoCard.Grid, numbers);
				player.IsFullCardDone = FullCardDone(player.BingoCard.Grid, numbers);
			}

			return new BingoGameModel()
			{
				DrawnNumber = nextNumber,
				Players = players
			};
		}

		private bool HorizontalLineDone(int?[][] grid, List<int?> drawnNumbers)
		{
			foreach (var row in grid)
			{
				var cleanNumbers = row.Where(number => number != null);
				var intersection = cleanNumbers.Intersect(drawnNumbers);

				if (intersection.Count() == cleanNumbers.Count())
				{
					return true;
				}
			}

			return false;
		}

		private bool FullCardDone(int?[][] grid, List<int?> drawnNumbers)
		{
			var gridNumbers = grid.SelectMany(row => row).Where(number => number != null).ToArray();
			var intersection = gridNumbers.Intersect(drawnNumbers);

			if (intersection.Count() == gridNumbers.Count())
			{
				return true;
			}

			return false;
		}
	}
}
