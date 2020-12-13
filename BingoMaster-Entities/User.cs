using System;

namespace BingoMaster_Entities
{
	public class User
	{
		public Guid Id { get; set; }
		public string UserName { get; set; }
		public string Salt { get; set; }
		public string Hash { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public Player Player { get; set; }
	}
}
