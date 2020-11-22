using BingoMaster_Logic.Interfaces;
using System;
using System.Linq;

namespace BingoMaster_Logic
{
	public class BingoNumberLogic : IBingoNumberLogic
	{
		#region Fields

		private readonly int[] _numbers;
		private int totalDrawnNumbers = 0;

		#endregion

		public BingoNumberLogic()
		{
			_numbers = GetRandomNumbers();
		}

		public int GetNextNumber()
		{
			var nextNumber = _numbers[totalDrawnNumbers];
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
