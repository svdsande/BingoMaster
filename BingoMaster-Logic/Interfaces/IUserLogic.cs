using BingoMaster_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic.Interfaces
{
	public interface IUserLogic
	{
		AuthenticatedUserModel Authenticate(AuthenticateUserModel authenticateUserModel);
	}
}
