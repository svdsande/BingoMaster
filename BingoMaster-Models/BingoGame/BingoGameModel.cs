using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Models
{
	public class BingoGameModel
	{
		public string Name { get; set; }
		public int DrawnNumber { get; set; }
		public IEnumerable<PlayerGameModel> Players { get; set; }
	}
}
