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

			_playerLogic = new PlayerLogic(context, _mapper);
		}

		[Fact]
		public void GetGamesForPlayer_NoId_ExceptionExpected()
		{
			var id = new Guid();
			Assert.Throws<ArgumentException>(() => _playerLogic.GetGamesForPlayer(id));
		}
	}
}
