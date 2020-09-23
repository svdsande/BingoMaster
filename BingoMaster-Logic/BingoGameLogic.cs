using BingoMaster_Logic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoMaster_Logic
{
	public class BingoGameLogic : IBingoGameLogic
	{
		private readonly int[] numbers;
		private int totalDrawnNumbers = 0;

		public BingoGameLogic()
		{
			numbers = GetRandomNumbers();
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
