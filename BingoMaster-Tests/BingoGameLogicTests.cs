using BingoMaster_Entities;
using BingoMaster_Logic;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using BingoMaster_Models.Player;
using Microsoft.EntityFrameworkCore;
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
		private readonly Guid _pearlJamGameId;
		private readonly Guid _fooFightersGameId;
		private readonly Guid _greenDayGameId;

		public BingoGameLogicTests()
		{
			_dbConnectionFactory = new DbConnectionFactory();
			_context = _dbConnectionFactory.CreateSQLiteContext();

			_creatorId = Guid.NewGuid();
			_normalPlayerId = Guid.NewGuid();
			_pearlJamGameId = Guid.NewGuid();
			_fooFightersGameId = Guid.NewGuid();
			_greenDayGameId = Guid.NewGuid();

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

			var gamePearlJam = new Game
			{
				Id = _pearlJamGameId,
				Name = "Pearl Jam Game",
				MaximumAmountOfPlayers = 3
			};

			var gameFooFighters = new Game
			{
				Id = _fooFightersGameId,
				Name = "Foo Fighters Game",
				MaximumAmountOfPlayers = 1
			};

			var gameGreenDay = new Game
			{
				Id = _greenDayGameId,
				Name = "Foo Fighters Game",
				MaximumAmountOfPlayers = 1
			};


			var gamePlayers = new List<GamePlayer>
			{
				new GamePlayer
				{
					Game = gamePearlJam,
					Player = players.First()
				},
				new GamePlayer
				{
					Game = gameFooFighters,
					Player = players.First()
				},
				new GamePlayer
				{
					Game = gameGreenDay,
					Player = players.First()
				}
			};

			_context.Players.AddRange(players);
			_context.Games.Add(gamePearlJam);
			_context.Games.Add(gameFooFighters);
			_context.Games.Add(gameGreenDay);
			_context.GamePlayers.AddRange(gamePlayers);

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
		public void CreateNewGame_CreatorNotFound_NullExpected()
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

			Assert.Null(_bingoGameLogic.CreateNewGame(input));
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
		[InlineData("00000000-0000-0000-0000-000000000000", "00ebf684-3797-473b-a503-a88c1c4cbb6d")]
		[InlineData("00ebf684-3797-473b-a503-a88c1c4cbb6d", "00000000-0000-0000-0000-000000000000")]
		public void JoinGame_InvalidData_ExceptionExpected(string gameId, string playerId)
		{
			Assert.Throws<ArgumentException>(() => _bingoGameLogic.JoinGame(Guid.Parse(gameId), Guid.Parse(playerId)));
		}

		[Fact]
		public void JoinGame_GameDoesNotExist_NoChangesExpected()
		{
			var gameDoesNotExistId = Guid.NewGuid();
			_bingoGameLogic.JoinGame(gameDoesNotExistId, _creatorId);

			var gamePlayer = _context.GamePlayers.SingleOrDefault(gamePlayer => gamePlayer.GameId == gameDoesNotExistId);

			Assert.Null(gamePlayer);
		}

		[Fact]
		public void JoinGame_PlayerAlreadyJoinedGame_NoChangesExpected()
		{
			_bingoGameLogic.JoinGame(_pearlJamGameId, _creatorId);

			var game = _context.Games
				.Include(game => game.GamePlayers)
				.SingleOrDefault(game => game.Id == _pearlJamGameId);

			Assert.NotNull(game.GamePlayers.SingleOrDefault(gamePlayer => gamePlayer.PlayerId == _creatorId));
		}

		[Fact]
		public void JoinGame_NewPlayerWantToJoinGame_PlayerShoulBeJoined()
		{
			_bingoGameLogic.JoinGame(_pearlJamGameId, _normalPlayerId);

			var game = _context.Games
				.Include(game => game.GamePlayers)
				.SingleOrDefault(game => game.Id == _pearlJamGameId);

			Assert.NotNull(game.GamePlayers.SingleOrDefault(gamePlayer => gamePlayer.PlayerId == _creatorId));
		}

		[Fact]
		public void JoinGame_MaximumAmountOfPlayersReached_PlayerShouldNotHaveJoined()
		{
			_bingoGameLogic.JoinGame(_fooFightersGameId, _normalPlayerId);

			var game = _context.Games
				.Include(game => game.GamePlayers)
				.SingleOrDefault(game => game.Id == _fooFightersGameId);

			Assert.Null(game.GamePlayers.SingleOrDefault(gamePlayer => gamePlayer.PlayerId == _normalPlayerId));
		}

		[Theory]
		[InlineData("00000000-0000-0000-0000-000000000000", "00ebf684-3797-473b-a503-a88c1c4cbb6d")]
		[InlineData("00ebf684-3797-473b-a503-a88c1c4cbb6d", "00000000-0000-0000-0000-000000000000")]
		public void LeaveGame_InvalidData_ExceptionExpected(string gameId, string playerId)
		{
			Assert.Throws<ArgumentException>(() => _bingoGameLogic.LeaveGame(Guid.Parse(gameId), Guid.Parse(playerId)));
		}

		[Fact]
		public void LeaveGame_GameDoesNotExist_NoChangesExpected()
		{
			var gameDoesNotExistId = Guid.NewGuid();
			_bingoGameLogic.LeaveGame(gameDoesNotExistId, _creatorId);

			var gamePlayer = _context.GamePlayers.SingleOrDefault(gamePlayer => gamePlayer.GameId == gameDoesNotExistId);

			Assert.Null(gamePlayer);
		}

		[Fact]
		public void LeaveGame_PlayerDoesNotParticipate_NoChangesExpected()
		{
			_bingoGameLogic.LeaveGame(_greenDayGameId, _normalPlayerId);

			var game = _context.Games
				.Include(game => game.GamePlayers)
				.SingleOrDefault(game => game.Id == _greenDayGameId);

			Assert.Null(game.GamePlayers.SingleOrDefault(gamePlayer => gamePlayer.PlayerId == _normalPlayerId));
		}

		[Fact]
		public void LeaveGame_PlayerDoesParticipate_PlayerLeftGame()
		{
			_bingoGameLogic.LeaveGame(_greenDayGameId, _creatorId);

			var game = _context.Games
				.Include(game => game.GamePlayers)
				.SingleOrDefault(game => game.Id == _greenDayGameId);

			Assert.Null(game.GamePlayers.SingleOrDefault(gamePlayer => gamePlayer.PlayerId == _creatorId));
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
