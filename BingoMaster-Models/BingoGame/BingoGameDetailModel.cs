using BingoMaster_Models.Player;
using System;
using System.Collections.Generic;

namespace BingoMaster_Models
{
	public class BingoGameDetailModel
	{
		public Guid CreatorId { get; set; }
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public int MaximumAmountOfPlayers { get; set; }
		public IEnumerable<PlayerModel> Players { get; set; }
		public int Size { get; set; }
		public bool IsCenterSquareFree { get; set; }
		public bool IsPrivateGame { get; set; }
	}
}
