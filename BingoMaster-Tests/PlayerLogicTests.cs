using BingoMaster_Entities;
using BingoMaster_Logic;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models.Player;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BingoMaster_Tests
{
	public class PlayerLogicTests : TestBase
	{
		private readonly IPlayerLogic _playerLogic;
		private readonly DbConnectionFactory _dbConnectionFactory;
		private readonly BingoMasterDbContext _context;
		private readonly Guid playerId;

		public PlayerLogicTests()
		{
			_dbConnectionFactory = new DbConnectionFactory();
			_context = _dbConnectionFactory.CreateSQLiteContext();

			playerId = Guid.NewGuid();

			var user = new User
			{
				FirstName = "Eddie",
				LastName = "Vedder",
				EmailAddress = "eddie-vedder@pearljam.com"
			};

			var player = new Player
			{
				Id = playerId,
				Name = "evedder",
				User = user
			};

			_context.Players.Add(player);
			_context.SaveChanges();

			_playerLogic = new PlayerLogic(_context, _mapper);
		}

		[Fact]
		public void GetGamesForPlayer_NoId_ExceptionExpected()
		{
			var id = new Guid();
			Assert.Throws<ArgumentException>(() => _playerLogic.GetGamesForPlayer(id));
		}

		[Fact]
		public void Update_PlayerNotFound_ExceptionExpected()
		{
			var model = new PlayerModel
			{
				Id = Guid.NewGuid(),
				Name = "evedder",
				Description = "Some description"
			};

			Assert.Throws<KeyNotFoundException>(() => _playerLogic.Update(model));
		}

		[Fact]
		public void Update_PlayerFound_Succeeds()
		{
			var model = new PlayerModel
			{
				Id = playerId,
				Name = "evedder",
				Description = "Some description"
			};

			_playerLogic.Update(model);

			Assert.Equal(model.Description, _context.Players.Find(playerId).Description);
		}

		[Theory]
		[InlineData("")]
		[InlineData("  ")]
		[InlineData(null)]
		public void PlayerNameUnique_NoUserName_ExceptionExpected(string userName)
		{
			Assert.Throws<ArgumentException>(() => _playerLogic.PlayerNameUnique(userName));
		}

		[Fact]
		public void PlayerNameUnique_UserNameTaken_NotUnique()
		{
			Assert.False(_playerLogic.PlayerNameUnique("evedder"));
		}

		[Fact]
		public void PlayerNameUnique_UserNameNotTaken_IsUnique()
		{
			Assert.True(_playerLogic.PlayerNameUnique("evedderBlack"));
		}
	}
}
