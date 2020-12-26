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

			jwtSettings = new JwtSettingsModel { Secret = "CzsQwFZ#DjM3NPa&rEN4F2E&2ZJ1Ysd3k2^TTz4Zo06w65B*07" };
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

			Assert.Throws<ArgumentException>(() => _userLogic.Authenticate(model));
		}

		[Fact]
		public void Authenticate_UserNotFound_Succeeds()
		{
			var model = new AuthenticateUserModel
			{
				EmailAddress = "jeff-ament@pearljam.com",
				Password = "password"
			};

			Assert.Null(_userLogic.Authenticate(model));
		}

		[Fact]
		public void Authenticate_PasswordVerifyFailes_ExceptionExpected()
		{
			var model = new AuthenticateUserModel
			{
				EmailAddress = "eddie-vedder@pearljam.com",
				Password = "password"
			};

			_passwordLogicMock.Setup(logic => logic.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(false);

			var exception = Assert.Throws<Exception>(() => _userLogic.Authenticate(model));
			Assert.Equal("Login failed", exception.Message);
		}

		[Fact]
		public void Authenticate_UserExists_Succeeds()
		{
			var model = new AuthenticateUserModel
			{
				EmailAddress = "eddie-vedder@pearljam.com",
				Password = "password"
			};

			_passwordLogicMock.Setup(logic => logic.VerifyPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);

			var actual = _userLogic.Authenticate(model);

			Assert.Equal("eddie-vedder@pearljam.com", actual.EmailAddress);
			Assert.Equal("Eddie", actual.FirstName);
			Assert.Equal("Vedder", actual.LastName);
			Assert.False(string.IsNullOrWhiteSpace(actual.Token));
		}

		[Theory]
		[InlineData("", "password", "username")]
		[InlineData("email", "", "username")]
		[InlineData("email", "password", "")]
		[InlineData("", "", "")]
		public void Register_NoPasswordOrUserNameOrEmail_ExceptionExpected(string emailAddress, string password, string userName)
		{
			var model = new RegisterUserModel
			{
				EmailAddress = emailAddress,
				Password = password,
				UserName = userName
			};

			var exception = Assert.Throws<ArgumentException>(() => _userLogic.Register(model));
			Assert.Equal("No email address, username or password provided", exception.Message);
		}

		[Fact]
		public void Register_EmailTaken_ExceptionExpected()
		{
			var model = new RegisterUserModel
			{
				EmailAddress = "eddie-vedder@pearljam.com",
				Password = "BlackAndAlive",
				UserName = "username"
			};

			Assert.Throws<UserAlreadyExistsException>(() => _userLogic.Register(model));
		}

		[Fact]
		public void Register_UserNameTaken_ExceptionExpected()
		{
			var model = new RegisterUserModel
			{
				EmailAddress = "eddie-vedder-black@pearljam.com",
				Password = "BlackAndAlive",
				UserName = "evedder"
			};

			Assert.Throws<UserAlreadyExistsException>(() => _userLogic.Register(model));
		}

		[Theory]
		[InlineData(PasswordStrength.Blank)]
		[InlineData(PasswordStrength.VeryWeak)]
		[InlineData(PasswordStrength.Weak)]
		[InlineData(PasswordStrength.Medium)]
		public void Register_PasswordTooWeak_ExceptionExpected(PasswordStrength passwordStrength)
		{
			var model = new RegisterUserModel
			{
				EmailAddress = "mike-mccready@pearljam.com",
				Password = "password",
				UserName = "evedderBlack"
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
				UserName = "McCready",
				FirstName = "Mike",
				LastName = "McCready"
			};

			_passwordLogicMock.Setup(logic => logic.GetPasswordStrength(It.IsAny<string>())).Returns(PasswordStrength.VeryStrong);

			var actual = _userLogic.Register(model);

			Assert.Equal("mike-mccready@pearljam.com", actual.EmailAddress);
			Assert.Equal("Mike", actual.FirstName);
			Assert.Equal("McCready", actual.LastName);
		}

		[Theory]
		[InlineData("")]
		[InlineData("  ")]
		[InlineData(null)]
		public void UserNameUnique_NoUserName_ExceptionExpected(string userName)
		{
			Assert.Throws<ArgumentException>(() => _userLogic.UserNameUnique(userName));
		}

		[Fact]
		public void UserNameUnique_UserNameTaken_NotUnique()
		{
			Assert.False(_userLogic.UserNameUnique("evedder"));
		}

		[Fact]
		public void UserNameUnique_UserNameNotTaken_IsUnique()
		{
			Assert.True(_userLogic.UserNameUnique("evedderBlack"));
		}

		[Theory]
		[InlineData("")]
		[InlineData("  ")]
		[InlineData(null)]
		public void EmailAddressUnique_NoEmailAddress_ExceptionExpected(string emailAddress)
		{
			Assert.Throws<ArgumentException>(() => _userLogic.EmailAddressUnique(emailAddress));
		}

		[Fact]
		public void EmailAddressUnique_EmailAddressTaken_NotUnique()
		{
			Assert.False(_userLogic.EmailAddressUnique("eddie-vedder@pearljam.com"));
		}

		[Fact]
		public void EmailAddressUnique_EmailAddressNotTaken_IsUnique()
		{
			Assert.True(_userLogic.EmailAddressUnique("eddie-vedder-black@pearljam.com"));
		}
	}
}
