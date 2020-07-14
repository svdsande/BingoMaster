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
                Grid = GetHtmlBingoCardGrid(bingoCardModel)
            });
        }

        private string GetHtmlBingoCardGrid(BingoCardCreationModel bingoCardModel)
        {
            var document = new HtmlDocument();
            var bingoCardHtml = BuildHtmlBingoCardGrid(bingoCardModel);
            document.LoadHtml(bingoCardHtml);

            return document.DocumentNode.OuterHtml;
        }

        private string BuildHtmlBingoCardGrid(BingoCardCreationModel bingoCardModel)
        {
            var grid = GenerateCardGrid(bingoCardModel.Size);

            if (bingoCardModel.IsCenterSquareFree)
            {
                SetCenterSquareFree(grid);
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append($@"<table style=""background-color:{bingoCardModel.BackgroundColor}"">");

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                stringBuilder.Append("<tr>");

                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j] != null)
                    {
                        stringBuilder.Append($@"<td style=""border: 1px solid {bingoCardModel.BorderColor}"">{grid[i, j]}</td>");
                    }
                     else
                    {
                        stringBuilder.Append($@"<td style=""border: 1px solid {bingoCardModel.BorderColor}"">X</td>");
                    }
                }

                stringBuilder.Append("</tr>");
            }

            stringBuilder.Append("</table>");

            return stringBuilder.ToString();
        }

        private void SetCenterSquareFree(int?[,] grid)
        {
            var lengthFirstDimension = grid.GetLength(0);
            var lengthSecondDimension = grid.GetLength(1);

            grid[lengthFirstDimension / 2, lengthSecondDimension / 2] = null;
        }

        private int?[,] GenerateCardGrid(int size)
        {
            var grid = new int?[size, size];
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
