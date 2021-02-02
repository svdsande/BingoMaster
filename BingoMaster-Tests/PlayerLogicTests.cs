using BingoMaster_Entities;
using BingoMaster_Logic;
using BingoMaster_Logic.Interfaces;
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

		public PlayerLogicTests()
		{
			_dbConnectionFactory = new DbConnectionFactory();
			var context = _dbConnectionFactory.CreateSQLiteContext();

			var user = new User
			{
				FirstName = "Eddie",
				LastName = "Vedder",
				EmailAddress = "eddie-vedder@pearljam.com"
			};

			var player = new Player
			{
				Name = "evedder",
				User = user
			};

			context.Players.Add(player);
			context.SaveChanges();

			_playerLogic = new PlayerLogic(context, _mapper);
		}

		[Fact]
		public void GetGamesForPlayer_NoId_ExceptionExpected()
		{
			var id = new Guid();
			Assert.Throws<ArgumentException>(() => _playerLogic.GetGamesForPlayer(id));
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
