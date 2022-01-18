using BingoMaster_Logic;
using BingoMaster_Models;
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
        [InlineData(3, 1, 9, 4)]
        [InlineData(3, 2, 9, 4)]
        [InlineData(4, 1, 16, 8)]
        [InlineData(4, 2, 16, 8)]
        [InlineData(5, 1, 25, 12)]
        [InlineData(5, 2, 25, 12)]
        public void GenerateBingoCards_FreeCenter_Succeeds(int size, int amount, int amountOfCells, int centerIndex)
        {
            var input = new BingoCardCreationModel()
            {
                Name = "Pearl Jam",
                Amount = amount,
                IsCenterSquareFree = true,
                Size = size,
            };

            var actual = _bingoCardLogic.GenerateBingoCards(input);

            Assert.Equal(amount, actual.Count());
            Assert.Equal(input.Name, actual.First().Name);
            Assert.Equal(size, actual.First().Grid.Length);
            Assert.Equal(amountOfCells, GetNumberOfCells(actual.First().Grid));
            Assert.Equal(centerIndex, GetCenterFreeIndex(actual.First().Grid));
        }

        [Fact]
        public void GenerateBingoCard_FreeCenter_Succeeds()
		{
            var input = new BingoCardCreationModel()
            {
                Name = "Pearl Jam",
                Amount = 1,
                IsCenterSquareFree = true,
                Size = 3,
            };

            var actual = _bingoCardLogic.GenerateBingoCard(input);

            Assert.Equal(input.Name, actual.Name);
            Assert.Equal(input.Size, actual.Grid.Length);
            Assert.Equal(9, GetNumberOfCells(actual.Grid));
            Assert.Equal(4, GetCenterFreeIndex(actual.Grid));
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
        public void GenerateBingoCards_NoName_ExceptionExpected()
        {
            var input = new BingoCardCreationModel()
            {
                Name = "",
                Amount = 3,
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

        private int GetNumberOfCells(int?[][] grid)
        {
            var count = 0;

            for (int i = 0; i < grid.Length; i++)
            {
                count += grid[i].Length;
            }

            return count;
        }

        private int GetCenterFreeIndex(int?[][] grid)
        {
            var flatGrid = grid.SelectMany(item => item);

            return flatGrid.Select((value, index) => new { value, index })
                .Where(pair => pair.value == null)
                .Select(pair => pair.index)
                .SingleOrDefault();
        }
    }
}
