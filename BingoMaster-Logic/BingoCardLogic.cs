using System;
using System.Collections;
using System.Linq;

namespace BingoMaster_Logic
{
    public class BingoCardLogic : IBingoCardLogic
    {
        public int[,,] GenerateCardGrids(int amount, int rows, int columns)
        {
            if (rows <= 0 || columns <= 0 || amount <= 0)
            {
                throw new ArgumentException("Invalid number of cards, rows or columns");
            }

            return GenerateGrids(amount, rows, columns);
        }

        private int[,,] GenerateGrids(int amount, int rows, int columns)
        {
            var grids = new int[amount, rows, columns];
            var random = new Random();

            for (int i = 0; i < amount; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    for (int z = 0; z < columns; z++)
                    {
                        grids[i, j, z] = random.Next(1, 91);
                    }
                }
            }

            return grids;
        }
    }
}
