using BingoMaster_API;
using BingoMaster_Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoMaster_Logic
{
    public class BingoCardLogic : IBingoCardLogic
    {
        public IEnumerable<BingoCardModel> GenerateBingoCards(BingoCardCreationModel bingoCardModel)
        {
            if (bingoCardModel == null || bingoCardModel.Amount <= 0 || bingoCardModel.Size <= 0)
            {
                throw new ArgumentException("Invalid number of cards or grid size");
            }

            return Enumerable.Range(1, bingoCardModel.Amount).Select(item => new BingoCardModel()
            {
                Name = bingoCardModel.Name,
                Grid = BuildBingoCardGrid(bingoCardModel)
            });
        }

        private int?[][] BuildBingoCardGrid(BingoCardCreationModel bingoCardModel)
        {
            var grid = GenerateCardGrid(bingoCardModel.Size);

            if (bingoCardModel.IsCenterSquareFree)
            {
                SetCenterSquareFree(grid);
            }

            return grid;
        }

        private void SetCenterSquareFree(int?[][] grid)
        {
            var height = grid.Length;
            var width = grid[0].Length;
            var amountOfCells = height * width;
            var centerCellIndex = (int)Math.Floor(amountOfCells / 2.0);

            grid[centerCellIndex / height][centerCellIndex % width] = null;
        }

        private int?[][] GenerateCardGrid(int size)
        {
            var grid = new int?[size][];
            var random = new Random();

            for (int i = 0; i < size; i++)
            {
                grid[i] = new int?[size];
                for (int j = 0; j < size; j++)
                {
                    grid[i][j] = random.Next(1, 91);
                }
            }

            return grid;
        }
    }
}
