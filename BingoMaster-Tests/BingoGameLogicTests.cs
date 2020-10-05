using BingoMaster_Logic;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BingoMaster_Tests
{
	public class BingoGameLogicTests
	{
		private readonly Mock<IBingoCardLogic> _bingoCardLogicMock;
		private readonly IBingoGameLogic _bingoGameLogic;

		public BingoGameLogicTests()
		{
			_bingoCardLogicMock = new Mock<IBingoCardLogic>(MockBehavior.Strict);
			_bingoGameLogic = new BingoGameLogic(_bingoCardLogicMock.Object);
		}

		[Fact]
		public void CreateNewGame_TwoPlayers_Succeeds()
		{
			var bingoCardModels = new List<BingoCardModel>()
			{
				new BingoCardModel() { Name = "Pearl Jam", Grid = new int?[3][] },
				new BingoCardModel() { Name = "Pearl Jam", Grid = new int?[3][] }
			};

			_bingoCardLogicMock.Setup(logic => logic.GenerateBingoCards(It.IsAny<BingoCardCreationModel>())).Returns(bingoCardModels);

			var actual = _bingoGameLogic.CreateNewGame("Pearl Jam", 2, 3);

			Assert.Equal(2, actual.Count());
			Assert.Equal("Pearl Jam", actual.First().Name);
			Assert.Equal(3, actual.First().Grid.Length);
		}

		[Theory]
		[InlineData("", 5, 3)]
		[InlineData("Pearl Jam", 0, 3)]
		[InlineData("Pearl Jam", 5, 0)]
		public void CreateNewGame_ZeroAmountOfPlayers_ExceptionExpected(string name, int amountOfPlayers, int size)
		{
			_bingoCardLogicMock.Setup(logic => logic.GenerateBingoCards(It.IsAny<BingoCardCreationModel>())).Returns(Enumerable.Empty<BingoCardModel>());
			Assert.Throws<ArgumentException>(() => _bingoGameLogic.CreateNewGame(name, amountOfPlayers, size));
		}
	}
}
