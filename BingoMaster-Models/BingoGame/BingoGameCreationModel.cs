using System.Collections.Generic;

namespace BingoMaster_Models
{
	public class BingoGameCreationModel
	{
		public string Name { get; set; }
		public IEnumerable<PlayerModel> Players { get; set; }
		public int Size { get; set; }
	}
}
