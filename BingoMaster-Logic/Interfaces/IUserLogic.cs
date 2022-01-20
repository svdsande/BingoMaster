using BingoMaster_Models;
using BingoMaster_Models.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic.Interfaces
{
	public interface IUserLogic
	{
		AuthenticatedUserModel Authenticate(AuthenticateUserModel authenticateUserModel);
		UserModel Register(RegisterUserModel registerUserModel);
		UserModel GetUserById(Guid id);
		void Update(UserModel userModel);
		bool EmailAddressUnique(string emailAddress);
	}
}
