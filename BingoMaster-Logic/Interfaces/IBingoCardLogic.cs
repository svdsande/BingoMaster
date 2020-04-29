using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic
{
    public interface IBingoCardLogic
    {
        /// <summary>
        /// Generate <paramref name="amount"/> of card grids with the specified number of <paramref name="rows"/> and <paramref name="columns"/>
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown when number of cards, rows or columns are either set to 0 or less</exception>
        /// <param name="amount">Amount of grids</param>
        /// <param name="rows">Number of rows</param>
        /// <param name="columns">Number of columns</param>
        /// <returns>A 3d array which represents the card grids</returns>
        int[,,] GenerateCardGrids(int amount, int rows, int columns);
    }
}
