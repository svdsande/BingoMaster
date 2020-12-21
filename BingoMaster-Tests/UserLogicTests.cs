using BingoMaster_Entities;
using BingoMaster_Logic;
using BingoMaster_Logic.Exceptions;
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
			_passwordLogicMock = new Mock<IPasswordLogic>(MockBehavior.Loose);
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

			Assert.Throws<UserAlreadyExistsException>(() => _userLogic.Register(model));
		}

		[Theory]
		[InlineData(PasswordStrength.Blank)]
		[InlineData(PasswordStrength.VeryWeak)]
		[InlineData(PasswordStrength.Weak)]
		public void Register_PasswordTooWeak_ExceptionExpected(PasswordStrength passwordStrength)
		{
			var model = new RegisterUserModel
			{
				EmailAddress = "mike-mccready@pearljam.com",
				Password = "password"
			};

			_passwordLogicMock.Setup(logic => logic.GetPasswordStrength(It.IsAny<string>())).Returns(passwordStrength);

			var exception = Assert.Throws<ArgumentException>(() => _userLogic.Register(model));
			Assert.Equal("Provided password too weak", exception.Message);
		}

		[Fact]
		public void Register_NewUser_Succeeds()
		{
			var model = new RegisterUserModel
			{
				EmailAddress = "mike-mccready@pearljam.com",
				Password = "$earl&am!2020",
				FirstName = "Mike",
				LastName = "McCready"
			};

			_passwordLogicMock.Setup(logic => logic.GetPasswordStrength(It.IsAny<string>())).Returns(PasswordStrength.VeryStrong);

			var actual = _userLogic.Register(model);

			Assert.Equal("mike-mccready@pearljam.com", actual.EmailAddress);
			Assert.Equal("Mike", actual.FirstName);
			Assert.Equal("McCready", actual.LastName);
		}
	}
}
