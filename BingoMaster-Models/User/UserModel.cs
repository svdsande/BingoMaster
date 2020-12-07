using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Models.User
{
	public class UserModel
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
	}
}
