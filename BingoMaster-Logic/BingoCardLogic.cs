using BingoMaster_API;
using BingoMaster_Models;
using System;
using System.Collections;
using System.Linq;

namespace BingoMaster_Logic
{
    public class BingoCardLogic : IBingoCardLogic
    {
        public BingoCardModel GenerateBingoCards(BingoCardCreationModel bingoCard)
        {
            if (bingoCard == null || bingoCard.Amount <= 0 || bingoCard.Size<= 0)
            {
                throw new ArgumentException("Invalid number of cards or grid size");
            }

            var bingoCardGrids = GenerateCardGrids(bingoCard.Amount, bingoCard.Size);

            return new BingoCardModel
            {
                Name = bingoCard.Name,
                Grids = bingoCardGrids
            };
        }

        private int[,,] GenerateCardGrids(int amount, int size)
        {
            var grids = new int[amount, size, size];
            var random = new Random();

            for (int i = 0; i < amount; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        grids[i, j, z] = random.Next(1, 91);
                    }
                }
            }

            return grids;
        }
    }
}
