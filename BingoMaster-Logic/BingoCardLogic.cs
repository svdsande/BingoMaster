using BingoMaster_API;
using BingoMaster_Models;
using Newtonsoft.Json;
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
                Grid = BuildHtmlBingoCardGrid(bingoCardModel)
            });
        }

        private string BuildHtmlBingoCardGrid(BingoCardCreationModel bingoCardModel)
        {
            var grid = GenerateCardGrid(bingoCardModel.Size);

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<table>");

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                stringBuilder.Append("<tr>");

                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    stringBuilder.Append($"<td>{grid[i, j]}</td>");
                }

                stringBuilder.Append("</tr>");
            }

            stringBuilder.Append("</table>");

            return stringBuilder.ToString();
        }

        private int[,] GenerateCardGrid(int size)
        {
            var grid = new int[size, size];
            var random = new Random();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = random.Next(1, 91);
                }
            }

            return grid;
        }
    }
}
