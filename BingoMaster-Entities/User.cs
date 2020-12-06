using System;

namespace BingoMaster_Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Emailaddress { get; set; }
		public Player Player { get; set; }
	}
}
