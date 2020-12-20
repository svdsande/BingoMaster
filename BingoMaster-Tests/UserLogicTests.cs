using BingoMaster_Entities;
using BingoMaster_Logic;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using BingoMaster_Models.User;
using Microsoft.Extensions.Options;
using Moq;
using System;
using Xunit;

namespace BingoMaster_Tests
{
	public class UserLogicTests
	{
		private readonly Mock<IPasswordLogic> _passwordLogicMock;
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
			context.SaveChanges();

			jwtSettings = new JwtSettingsModel { Secret = "YellowLedbetter" };
			_jwtSettingsModelMock = new Mock<IOptions<JwtSettingsModel>>(MockBehavior.Strict);
			_jwtSettingsModelMock.Setup(setting => setting.Value).Returns(jwtSettings);
			_passwordLogicMock = new Mock<IPasswordLogic>(MockBehavior.Strict);
			_userLogic = new UserLogic(_jwtSettingsModelMock.Object, context, _passwordLogicMock.Object);
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

		[Theory]
		[InlineData("", "password")]
		[InlineData("email", "")]
		[InlineData("", "")]
		public void Register_NoPasswordOrEmail_ExceptionExpected(string emailAddress, string password)
		{
			var model = new RegisterUserModel
			{
				EmailAddress = emailAddress,
				Password = password
			};

			var exception = Assert.Throws<ArgumentException>(() => _userLogic.Register(model));
			Assert.Equal("No email address or password provided", exception.Message);
		}

		[Fact]
		public void Register_EmailTaken_ExceptionExpected()
		{
			var model = new RegisterUserModel
			{
				EmailAddress = "eddie-vedder@pearljam.com",
				Password = "BlackAndAlive"
			};

			var exception = Assert.Throws<ArgumentException>(() => _userLogic.Register(model));
			Assert.Equal("Email address is already taken", exception.Message);
		}

		[Fact]
		public void Register_EmailNotTaken_Success()
		{
			var model = new RegisterUserModel
			{
				EmailAddress = "eddie-vedder@pearljam.com",
				Password = "BlackAndAlive"
			};

			var exception = Assert.Throws<ArgumentException>(() => _userLogic.Register(model));
			Assert.Equal("Email address is already taken", exception.Message);
		}
	}
}
