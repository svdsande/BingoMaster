using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Entities
{
	public class Game
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int MaximumAmountOfPlayers { get; set; }
		public bool Private { get; set; }
		public bool CenterSquareFree { get; set; }
		public Player Creator { get; set; }
		public Player Winner { get; set; }
		public int Grid { get; set; }
		public DateTime Date { get; set; }
		public ICollection<GamePlayer> GamePlayers { get; set; } 
	}
}
