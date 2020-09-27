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
		public void CreateNewGame_ZeroAmountOfPlayers_ExceptionExpected()
		{
			_bingoCardLogicMock.Setup(logic => logic.GenerateBingoCards(It.IsAny<BingoCardCreationModel>())).Returns(Enumerable.Empty<BingoCardModel>());
			Assert.Throws<ArgumentException>(() => _bingoGameLogic.CreateNewGame("Pearl Jam", 0));
		}
	}
}
