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
		void Register(RegisterUserModel registerUserModel);
		UserModel GetUserById(Guid id);
	}
}
