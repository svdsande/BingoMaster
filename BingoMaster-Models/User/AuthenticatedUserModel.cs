using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Models
{
	public class AuthenticatedUserModel
	{
		public Guid Id { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public string UserName { get; set; }
		public string Token { get; set; }
	}
}
