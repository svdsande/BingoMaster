using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Models.User
{
	public class RegisterUserModel
	{
		public string PlayerName { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string Password { get; set; }
	}
}
