using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Entities
{
	public class Game
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int Grid { get; set; }
		public DateTime Date { get; set; }
		public ICollection<GamePlayer> GamePlayers { get; set; } 
	}
}
