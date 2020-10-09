using BingoMaster_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic
{
    public interface IBingoCardLogic
    {
        /// <summary>
        /// Generate bingo cards based on the information which is stored in <paramref name="bingoCard"/>
        /// Every bingo card consists of a name along with a grid (jagged int array) filled with random integers.
        /// The random integers range from 1 until 90.
        /// </summary>
        /// <param name="bingoCard">Model which contains all the necessary information about the desired bingo card(s)</param>
        /// <exception cref="ArgumentException">Thrown when the amount or size is less then or equal to 0</exception>
        /// <returns>Collection of the generated bingo cards</returns>
        IEnumerable<BingoCardModel> GenerateBingoCards(BingoCardCreationModel bingoCard);
        BingoCardModel GenerateBingoCard(BingoCardCreationModel bingoCard);
    }
}
