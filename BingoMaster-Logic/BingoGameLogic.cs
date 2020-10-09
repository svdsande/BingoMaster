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
		private readonly int[] numbers;
		private int totalDrawnNumbers = 0;

		#endregion

		public BingoGameLogic(IBingoCardLogic bingoCardLogic)
		{
			_bingoCardLogic = bingoCardLogic;
			numbers = GetRandomNumbers();
		}

		public BingoGameModel CreateNewGame(BingoGameCreationModel gameCreationModel)
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
				DrawnNumber = GetNextNumber(),
				Players = gameCreationModel.Players
			};
		}

		public BingoGameModel PlayRound(IEnumerable<PlayerModel> players, int[] drawnNumbers)
		{
			if (players.Count() <= 0 || drawnNumbers.Length <= 0)
			{
				throw new ArgumentException("Invalid number of players or drawn numbers");
			}

			var nextNumber = GetNextNumber();
			var numbers = drawnNumbers.Select(number => (int?)number).ToArray();

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

		public int GetNextNumber()
		{
			var nextNumber = numbers[totalDrawnNumbers];
			totalDrawnNumbers++;

			return nextNumber;
		}

		private int[] GetRandomNumbers()
		{
			var numbers = Enumerable.Range(1, 90).ToArray();
			var random = new Random();

			for (int i = 0; i < numbers.Length; i++)
			{
				int randomIndex = random.Next(numbers.Length);
				int temp = numbers[randomIndex];
				numbers[randomIndex] = numbers[i];
				numbers[i] = temp;
			}

			return numbers;
		}

		private bool HorizontalLineDone(int?[][] grid, int?[] drawnNumbers)
		{
			foreach (var row in grid)
			{
				var intersection = row.Intersect(drawnNumbers);

				if (intersection.Count() == row.Length)
				{
					return true;
				}
			}

			return false;
		}

		private bool FullCardDone(int?[][] grid, int?[] drawnNumbers)
		{
			var gridNumbers = grid.SelectMany(row => row).ToArray();
			var intersection = gridNumbers.Intersect(drawnNumbers);

			if (intersection.Count() == drawnNumbers.Length)
			{
				return true;
			}

			return false;
		}
	}
}
