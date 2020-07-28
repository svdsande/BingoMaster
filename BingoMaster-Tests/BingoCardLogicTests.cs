using BingoMaster_Logic;
using BingoMaster_Models;
using HtmlAgilityPack;
using System;
using System.Linq;
using Xunit;

namespace BingoMaster_Tests
{
    public class BingoCardLogicTests
    {
        private readonly IBingoCardLogic _bingoCardLogic;

        public BingoCardLogicTests()
        {
            _bingoCardLogic = new BingoCardLogic();
        }

        [Theory]
        [InlineData(3, 1)]
        [InlineData(3, 2)]
        [InlineData(4, 1)]
        [InlineData(4, 2)]
        [InlineData(5, 1)]
        [InlineData(5, 2)]
        public void GenerateBingoCards_FreeCenter_Succeeds(int size, int amount)
        {
            var input = new BingoCardCreationModel()
            {
                Name = "Pearl Jam",
                Amount = amount,
                IsCenterSquareFree = true,
                Size = size,
                BackgroundColor = "#ADD8E6",
                BorderColor = "#FFFFFF"
            };

            var actual = _bingoCardLogic.GenerateBingoCards(input);
            var document = new HtmlDocument();
            document.LoadHtml(actual.First().Grid);

            var amountOfCells = size * size;
            var centerSquareIndex = (int)Math.Ceiling((double)amountOfCells / 2 - 1);

            Assert.Equal(amount, actual.Count());
            Assert.Equal(input.Name, actual.First().Name);
            Assert.Equal(size, document.DocumentNode.SelectNodes("//tr").Count);
            Assert.Equal(amountOfCells, document.DocumentNode.SelectNodes("//tr/td").Count);
            Assert.Equal("X", document.DocumentNode.SelectNodes("//tr/td")[centerSquareIndex].InnerText);
            Assert.Equal("background-color:#ADD8E6", document.DocumentNode.SelectSingleNode("table").Attributes["style"].Value);
            Assert.All(document.DocumentNode.SelectNodes("//tr/td"), node => Assert.Equal("border: 1px solid #FFFFFF", node.Attributes["style"].Value));
        }

        [Fact]
        public void GenerateBingoCards_OneCard_3x3_Succeeds()
        {
            var input = new BingoCardCreationModel()
            {
                Name = "Pearl Jam",
                Amount = 1,
                IsCenterSquareFree = false,
                Size = 3,
                BackgroundColor = "#ADD8E6",
                BorderColor = "#FFFFFF"
            };

            var actual = _bingoCardLogic.GenerateBingoCards(input);
            var document = new HtmlDocument();
            document.LoadHtml(actual.First().Grid);

            Assert.Single(actual);
            Assert.Equal(input.Name, actual.First().Name);
            Assert.Equal(3, document.DocumentNode.SelectNodes("//tr").Count);
            Assert.Equal(9, document.DocumentNode.SelectNodes("//tr/td").Count);
            Assert.Equal("background-color:#ADD8E6", document.DocumentNode.SelectSingleNode("table").Attributes["style"].Value);
            Assert.All(document.DocumentNode.SelectNodes("//tr/td"), node => Assert.Equal("border: 1px solid #FFFFFF", node.Attributes["style"].Value));
        }

        [Fact]
        public void GenerateBingoCards_NullInput_ExceptionExpected()
        {
            Assert.Throws<ArgumentException>(() => _bingoCardLogic.GenerateBingoCards(null));
        }

        [Fact]
        public void GenerateBingoCards_ZeroAmount_ExceptionExpected()
        {
            var input = new BingoCardCreationModel()
            {
                Name = "Pearl Jam",
                Amount = 0,
                IsCenterSquareFree = false,
                Size = 3
            };

            Assert.Throws<ArgumentException>(() => _bingoCardLogic.GenerateBingoCards(input));
        }

        [Fact]
        public void GenerateBingoCards_ZeroSize_ExceptionExpected()
        {
            var input = new BingoCardCreationModel()
            {
                Name = "Pearl Jam",
                Amount = 3,
                IsCenterSquareFree = false,
                Size = 0
            };

            Assert.Throws<ArgumentException>(() => _bingoCardLogic.GenerateBingoCards(input));
        }
    }
}
