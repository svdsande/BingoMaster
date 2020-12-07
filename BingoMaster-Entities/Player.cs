﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Entities
{
	public class Player
	{
		public Guid Id { get; set; }
		public ICollection<GamePlayer> GamePlayers { get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }
	}
}
