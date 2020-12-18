using BingoMaster_Entities;
using BingoMaster_Logic;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BingoMaster_Tests
{
	public class UserLogicTests
	{
		private readonly Mock<IOptions<JwtSettingsModel>> _jwtSettingsModelMock;
		private readonly JwtSettingsModel jwtSettings;
		private readonly IUserLogic _userLogic;
		private readonly DbConnectionFactory _dbConnectionFactory;

		public UserLogicTests()
		{
			_dbConnectionFactory = new DbConnectionFactory();
			var context = _dbConnectionFactory.CreateSQLiteContext();

			var user = new User
			{
				FirstName = "Eddie",
				LastName = "Vedder",
				EmailAddress = "eddie-vedder@pearljam.com",
				UserName = "evedder"
			};

			context.Users.Add(user);

			jwtSettings = new JwtSettingsModel { Secret = "YellowLedbetter" };
			_jwtSettingsModelMock = new Mock<IOptions<JwtSettingsModel>>(MockBehavior.Strict);
			_jwtSettingsModelMock.Setup(setting => setting.Value).Returns(jwtSettings);
			_userLogic = new UserLogic(_jwtSettingsModelMock.Object, context);
		}

		[Theory]
		[InlineData("", "password")]
		[InlineData("email", "")]
		[InlineData("", "")]
		public void Authenticate_NoPasswordOrEmail_ExceptionExpected(string emailAddress, string password)
		{
			var model = new AuthenticateUserModel
			{
				EmailAddress = emailAddress,
				Password = password
			};

			var exception = Assert.Throws<ArgumentException>(() => _userLogic.Authenticate(model));
		}
	}
}
