using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace BingoMaster_Logic.Exceptions
{
	public class UserAlreadyExistsException : Exception
	{
		public UserAlreadyExistsException()
		{
		}

		public UserAlreadyExistsException(string message) : base(message)
		{
		}

		public UserAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}
