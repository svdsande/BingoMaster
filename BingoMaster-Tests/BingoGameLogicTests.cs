using BingoMaster_Entities;
using BingoMaster_Logic;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using BingoMaster_Models.Player;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace BingoMaster_Tests
{
	public class BingoGameLogicTests : TestBase
	{
		private readonly Mock<IBingoCardLogic> _bingoCardLogicMock;
		private readonly Mock<IBingoNumberLogic> _bingoNumberLogicMock;
		private readonly DbConnectionFactory _dbConnectionFactory;
		private readonly BingoMasterDbContext _context;
		private readonly IBingoGameLogic _bingoGameLogic;
		private readonly Guid _creatorId;
		private readonly Guid _normalPlayerId;

		public BingoGameLogicTests()
		{
			_dbConnectionFactory = new DbConnectionFactory();
			_context = _dbConnectionFactory.CreateSQLiteContext();

			_creatorId = Guid.NewGuid();
			_normalPlayerId = Guid.NewGuid();

			var user = new User
			{
				FirstName = "Eddie",
				LastName = "Vedder",
				EmailAddress = "eddie-vedder@pearljam.com"
			};

			var normalUser = new User
			{
				FirstName = "Mike",
				LastName = "MC Cready",
				EmailAddress = "mike-mccready@pearljam.com"
			};

			var players = new List<Player>
			{
				new Player
				{
					Id = _creatorId,
					Name = "evedder",
					User = user
				},
				new Player
				{
					Id = _normalPlayerId,
					Name = "mikemccready",
					User = normalUser
				}
			};


			_context.Players.AddRange(players);

			_context.SaveChanges();

			_bingoCardLogicMock = new Mock<IBingoCardLogic>(MockBehavior.Strict);
			_bingoNumberLogicMock = new Mock<IBingoNumberLogic>(MockBehavior.Strict);
			_bingoGameLogic = new BingoGameLogic(_bingoCardLogicMock.Object, _bingoNumberLogicMock.Object, _context, _mapper);
		}

		[Theory]
		[InlineData("", 2, 2, 3)]
		[InlineData("Pearl Jam", 0, 2, 3)]
		[InlineData("Pearl Jam", 2, 2, 0)]
		[InlineData("Pearl Jam", 2, 3, 3)]
		public void CreateNewGame_InvalidDate_ExceptionExpected(string name, int maximumAmountOfPlayers, int amountOfPlayers, int size)
		{
			var input = new BingoGameDetailModel()
			{
				Name = name,
				MaximumAmountOfPlayers = maximumAmountOfPlayers,
				Players = Enumerable.Range(1, amountOfPlayers).Select(item => new PlayerModel()).ToArray(),
				Size = size
			};

			Assert.Throws<ArgumentException>(() => _bingoGameLogic.CreateNewGame(input));
		}

		[Fact]
		public void CreateNewGame_DateInThePast_ExceptionExpected()
		{
			var input = new BingoGameDetailModel()
			{
				Name = "Pearl Jam",
				MaximumAmountOfPlayers = 3,
				Players = Enumerable.Range(1, 2).Select(item => new PlayerModel()).ToArray(),
				Size = 3,
				CreatorId = Guid.NewGuid(),
				Date = new DateTime(1994, 12, 3)
			};

			Assert.Throws<ArgumentOutOfRangeException>(() => _bingoGameLogic.CreateNewGame(input));
		}

		[Fact]
		public void CreateNewGame_CreatorNotFound_ExceptionExpected()
		{
			var input = new BingoGameDetailModel()
			{
				Name = "Pearl Jam",
				MaximumAmountOfPlayers = 3,
				Players = Enumerable.Range(1, 2).Select(item => new PlayerModel()).ToArray(),
				Size = 3,
				CreatorId = Guid.NewGuid(),
				Date = DateTime.Now.AddMonths(1)
			};

			Assert.Throws<KeyNotFoundException>(() => _bingoGameLogic.CreateNewGame(input));
		}

		[Fact]
		public void CreateNewGame_NoPlayers_Succeeds()
		{
			var input = new BingoGameDetailModel()
			{
				Name = "Pearl Jam",
				MaximumAmountOfPlayers = 3,
				Size = 3,
				CreatorId = _creatorId,
				Date = DateTime.Now.AddMonths(1)
			};

			var expected = new BingoGameModel()
			{
				Name = "Pearl Jam",
			};

			var actual = _bingoGameLogic.CreateNewGame(input);

			Assert.Equal(expected.Name, actual.Name);
			Assert.Empty(actual.Players);
		}

		[Fact]
		public void CreateNewGame_SomePlayers_Succeeds()
		{
			var input = new BingoGameDetailModel()
			{
				Name = "Pearl Jam",
				MaximumAmountOfPlayers = 3,
				Size = 3,
				Players = new List<PlayerModel>
				{
					new PlayerModel
					{
						Id = _normalPlayerId,
						Name = "mikemccready"
					},
					new PlayerModel
					{
						Id = _creatorId,
						Name = "evedder"
					}
				},
				CreatorId = _creatorId,
				Date = DateTime.Now.AddMonths(1)
			};

			var expected = new BingoGameModel()
			{
				Name = "Pearl Jam",
				Players = new List<PlayerGameModel>
				{
					new PlayerGameModel
					{
						Name = "evedder",
					},
					new PlayerGameModel
					{
						Name = "mikemccready"
					}
				}
			};

			var actual = _bingoGameLogic.CreateNewGame(input);

			Assert.Equal(expected.Name, actual.Name);
			Assert.Equal(expected.Players.Count(), actual.Players.Count());
		}

		[Theory]
		[InlineData(new int[] { 1, 2, 3 })]
		[InlineData(new int[] { 3, 1, 2 })]
		[InlineData(new int[] { 2, 3, 1 })]
		[InlineData(new int[] { 4, 6 })]
		[InlineData(new int[] { 7 })]
		public void PlayRound_HorizontalLineDone_Succeeds(int[] drawnNumbers)
		{
			var input = new List<PlayerGameModel>()
			{
				new PlayerGameModel() 
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
			var input = new List<PlayerGameModel>()
			{
				new PlayerGameModel()
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
			var input = new List<PlayerGameModel>()
			{
				new PlayerGameModel()
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
			var input = new List<PlayerGameModel>()
			{
				new PlayerGameModel()
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

		[Theory]
		[InlineData(new int[] { 1, 2, 3, 4, 6 }, 7)]
		[InlineData(new int[] { 3, 6, 1, 2, 7 }, 4)]
		[InlineData(new int[] { 2, 4, 3, 6, 7 }, 1)]
		public void PlayRound_FullCardDone_NextDrawnNumber_Succeeds(int[] drawnNumbers, int nextNumberToBeDrawn)
		{
			var input = new List<PlayerGameModel>()
			{
				new PlayerGameModel()
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

			_bingoNumberLogicMock.Setup(logic => logic.GetNextNumber()).Returns(nextNumberToBeDrawn);

			var actual = _bingoGameLogic.PlayRound(input, drawnNumbers);

			Assert.True(actual.Players.First().IsHorizontalLineDone);
			Assert.True(actual.Players.First().IsFullCardDone);
		}

		[Theory]
		[InlineData(new int[] { 1, 2, 3, 4, 6, 7 })]
		[InlineData(new int[] { 7, 6, 4, 3, 2, 1 })]
		[InlineData(new int[] { 3, 6, 1, 7, 2, 4 })]
		public void PlayRound_FullCardDone_Succeeds(int[] drawnNumbers)
		{
			var input = new List<PlayerGameModel>()
			{
				new PlayerGameModel()
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
			Assert.True(actual.Players.First().IsFullCardDone);
		}

		[Theory]
		[InlineData(new int[] { 1, 2, 3 })]
		[InlineData(new int[] { 7, 3, 2, 1 })]
		[InlineData(new int[] { 3, 1, 7, 2, 4 })]
		public void PlayRound_FullCardDone_Failes(int[] drawnNumbers)
		{
			var input = new List<PlayerGameModel>()
			{
				new PlayerGameModel()
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
			Assert.False(actual.Players.First().IsFullCardDone);
		}

		[Theory]
		[InlineData(new int[] { 1, 2, 3, 4}, 6)]
		[InlineData(new int[] { 4, 6, 7, 2 }, 1)]
		public void PlayRound_FullCardDone_NextDrawnNumber_Failes(int[] drawnNumbers, int nextNumberToBeDrawn)
		{
			var input = new List<PlayerGameModel>()
			{
				new PlayerGameModel()
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

			_bingoNumberLogicMock.Setup(logic => logic.GetNextNumber()).Returns(nextNumberToBeDrawn);

			var actual = _bingoGameLogic.PlayRound(input, drawnNumbers);

			Assert.True(actual.Players.First().IsHorizontalLineDone);
			Assert.False(actual.Players.First().IsFullCardDone);
		}

		[Fact]
		public void PlayRound_NoPlayers_ExceptionExpected()
		{
			var input = new List<PlayerGameModel>() { };
			Assert.Throws<ArgumentException>(() => _bingoGameLogic.PlayRound(input, new int[] { 1, 2, 3 }));
		}

		[Fact]
		public void PlayRound_NoDrawnNumbers_ExceptionExpected()
		{
			var input = new List<PlayerGameModel>()
			{
				new PlayerGameModel() { Name = "Eddie Vedder" },
				new PlayerGameModel() { Name = "Mike McCready" }
			};
			Assert.Throws<ArgumentException>(() => _bingoGameLogic.PlayRound(input, new int[] {}));
		}
	}
}
