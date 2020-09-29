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

		public IEnumerable<BingoCardModel> CreateNewGame(string name, int amountOfPlayers, int size)
		{
			if (string.IsNullOrWhiteSpace(name) || amountOfPlayers <= 0 || size <= 0)
			{
				throw new ArgumentException("Invalid number players or grid size");
			}

			return _bingoCardLogic.GenerateBingoCards(new BingoCardCreationModel()
			{
				Amount = amountOfPlayers,
				Name = name,
				IsCenterSquareFree = true,
				Size = size
			});
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
	}
}
