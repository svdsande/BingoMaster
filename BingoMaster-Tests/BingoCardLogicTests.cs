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

        [Fact]
        public void GenerateBingoCards_OneCard_Succeeds()
        {
            var input = new BingoCardCreationModel()
            {
                Name = "Pearl Jam",
                Amount = 1,
                IsCenterSquareFree = true,
                Size = 3
            };

            var actual = _bingoCardLogic.GenerateBingoCards(input);
            var document = new HtmlDocument();
            document.LoadHtml(actual.First().Grid);

            Assert.Single(actual);
            Assert.Equal(input.Name, actual.First().Name);
            Assert.Equal(3, document.DocumentNode.SelectNodes("//tr").Count);
            Assert.Equal(9, document.DocumentNode.SelectNodes("//tr/td").Count);
            Assert.Equal("X", document.DocumentNode.SelectNodes("//tr/td")[4].InnerText);
        }

        [Fact]
        public void GenerateBingoCards_OneCard_FreeCenter_Succeeds()
        {
            var input = new BingoCardCreationModel()
            {
                Name = "Pearl Jam",
                Amount = 1,
                IsCenterSquareFree = false,
                Size = 3
            };

            var actual = _bingoCardLogic.GenerateBingoCards(input);
            var document = new HtmlDocument();
            document.LoadHtml(actual.First().Grid);

            Assert.Single(actual);
            Assert.Equal(input.Name, actual.First().Name);
            Assert.Equal(3, document.DocumentNode.SelectNodes("//tr").Count);
            Assert.Equal(9, document.DocumentNode.SelectNodes("//tr/td").Count);
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
