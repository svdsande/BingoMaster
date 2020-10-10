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
		private readonly Mock<IBingoNumberLogic> _bingoNumberLogicMock;
		private readonly IBingoGameLogic _bingoGameLogic;

		public BingoGameLogicTests()
		{
			_bingoCardLogicMock = new Mock<IBingoCardLogic>(MockBehavior.Strict);
			_bingoNumberLogicMock = new Mock<IBingoNumberLogic>(MockBehavior.Strict);
			_bingoGameLogic = new BingoGameLogic(_bingoCardLogicMock.Object, _bingoNumberLogicMock.Object);
		}

		[Fact]
		public void CreateNewGame_TwoPlayers_Succeeds()
		{
			var input = new BingoGameCreationModel()
			{
				Name = "Pearl Jam",
				Players = new List<PlayerModel>()
				{
					new PlayerModel() { Name = "Eddie Vedder" },
					new PlayerModel() { Name = "Mike McCready" }
				},
				Size = 3
			};

			_bingoCardLogicMock.Setup(logic => logic.GenerateBingoCard(It.IsAny<BingoCardCreationModel>())).Returns(new BingoCardModel()
			{
				Name = "Pearl Jam",
				Grid = new int?[][]
				{
					new int?[] { 1, 2, 3 },
					new int?[] { 4, 5, 6 },
					new int?[] { 7, 8, 9 }
				}
			});

			_bingoNumberLogicMock.Setup(logic => logic.GetNextNumber()).Returns(1);

			var actual = _bingoGameLogic.CreateNewGame(input);

			Assert.Equal(2, actual.Players.Count());
			Assert.Equal("Eddie Vedder", actual.Players.First().Name);
			Assert.Equal(3, actual.Players.First().BingoCard.Grid.Length);
		}

		[Fact]
		public void CreateNewGame_ZeroAmountOfPlayers_ExceptionExpected()
		{
			var input = new BingoGameCreationModel()
			{
				Name = "Pearl Jam",
				Players = new List<PlayerModel>() { },
				Size = 3
			};

			Assert.Throws<ArgumentException>(() => _bingoGameLogic.CreateNewGame(input));
		}

		[Fact]
		public void CreateNewGame_ZeroSize_ExceptionExpected()
		{
			var input = new BingoGameCreationModel()
			{
				Name = "Pearl Jam",
				Players = new List<PlayerModel>()
				{
					new PlayerModel() { Name = "Eddie Vedder" },
					new PlayerModel() { Name = "Mike McCready" }
				},
				Size = 0
			};

			Assert.Throws<ArgumentException>(() => _bingoGameLogic.CreateNewGame(input));
		}

		[Fact]
		public void CreateNewGame_NoName_ExceptionExpected()
		{
			var input = new BingoGameCreationModel()
			{
				Name = "",
				Players = new List<PlayerModel>()
				{
					new PlayerModel() { Name = "Eddie Vedder" },
					new PlayerModel() { Name = "Mike McCready" }
				},
				Size = 0
			};

			Assert.Throws<ArgumentException>(() => _bingoGameLogic.CreateNewGame(input));
		}

		[Theory]
		[InlineData(new int[] { 1, 2, 3 })]
		[InlineData(new int[] { 3, 1, 2 })]
		[InlineData(new int[] { 2, 3, 1 })]
		[InlineData(new int[] { 4, 6 })]
		[InlineData(new int[] { 7 })]
		public void PlayRound_HorizontalLineDone_Succeeds(int[] drawnNumbers)
		{
			var input = new List<PlayerModel>()
			{
				new PlayerModel() 
				{ 
					Name = "Eddie Vedder",
					BingoCard = new BingoCardModel()
					{
						Grid = new int?[][]
						{
							new int?[] { 1, 2, 3 },
							new int?[] { 4, null, 6 },
							new int?[] { 7, null, null }
						}
					}
				}
			};

			_bingoNumberLogicMock.Setup(logic => logic.GetNextNumber()).Returns(15);

			var actual = _bingoGameLogic.PlayRound(input, drawnNumbers);

			Assert.True(actual.Players.First().IsHorizontalLineDone);
		}

		[Theory]
		[InlineData(new int[] { 1, 2 }, 3)]
		[InlineData(new int[] { 7, 9 }, 8)]
		[InlineData(new int[] { 4 }, 6)]
		public void PlayRound_HorizontalLineDone_NextDrawnNumber_Succeeds(int[] drawnNumbers, int nextNumberToBeDrawn)
		{
			var input = new List<PlayerModel>()
			{
				new PlayerModel()
				{
					Name = "Eddie Vedder",
					BingoCard = new BingoCardModel()
					{
						Grid = new int?[][]
						{
							new int?[] { 1, 2, 3 },
							new int?[] { 4, null, 6 },
							new int?[] { 7, 8, 9 }
						}
					}
				}
			};

			_bingoNumberLogicMock.Setup(logic => logic.GetNextNumber()).Returns(nextNumberToBeDrawn);

			var actual = _bingoGameLogic.PlayRound(input, drawnNumbers);

			Assert.True(actual.Players.First().IsHorizontalLineDone);
		}

		[Theory]
		[InlineData(new int[] { 1, 2 })]
		[InlineData(new int[] { 4, 5, 9 })]
		[InlineData(new int[] { 7, 5, 6 })]
		[InlineData(new int[] { 2, 3 })]
		public void PlayRound_HorizontalLineDone_Failes(int[] drawnNumbers)
		{
			var input = new List<PlayerModel>()
			{
				new PlayerModel()
				{
					Name = "Eddie Vedder",
					BingoCard = new BingoCardModel()
					{
						Grid = new int?[][]
						{
							new int?[] { 1, 2, 3 },
							new int?[] { 4, 5, 6 },
							new int?[] { 7, null, 9 }
						}
					}
				}
			};

			_bingoNumberLogicMock.Setup(logic => logic.GetNextNumber()).Returns(15);

			var actual = _bingoGameLogic.PlayRound(input, drawnNumbers);

			Assert.False(actual.Players.First().IsHorizontalLineDone);
		}

		[Theory]
		[InlineData(new int[] { 1, 2 }, 4)]
		[InlineData(new int[] { 7 }, 8)]
		[InlineData(new int[] { 1, 2, 4, 7 }, 8)]
		public void PlayRound_HorizontalLineDone_NextDrawnNumber_Failes(int[] drawnNumbers, int nextNumberToBeDrawn)
		{
			var input = new List<PlayerModel>()
			{
				new PlayerModel()
				{
					Name = "Eddie Vedder",
					BingoCard = new BingoCardModel()
					{
						Grid = new int?[][]
						{
							new int?[] { 1, 2, 3 },
							new int?[] { 4, null, 6 },
							new int?[] { 7, 8, 9 }
						}
					}
				}
			};

			_bingoNumberLogicMock.Setup(logic => logic.GetNextNumber()).Returns(nextNumberToBeDrawn);

			var actual = _bingoGameLogic.PlayRound(input, drawnNumbers);

			Assert.False(actual.Players.First().IsHorizontalLineDone);
		}

		[Fact]
		public void PlayRound_NoPlayers_ExceptionExpected()
		{
			var input = new List<PlayerModel>() { };
			Assert.Throws<ArgumentException>(() => _bingoGameLogic.PlayRound(input, new int[] { 1, 2, 3 }));
		}

		[Fact]
		public void PlayRound_NoDrawnNumbers_ExceptionExpected()
		{
			var input = new List<PlayerModel>()
			{
				new PlayerModel() { Name = "Eddie Vedder" },
				new PlayerModel() { Name = "Mike McCready" }
			};
			Assert.Throws<ArgumentException>(() => _bingoGameLogic.PlayRound(input, new int[] {}));
		}
	}
}
