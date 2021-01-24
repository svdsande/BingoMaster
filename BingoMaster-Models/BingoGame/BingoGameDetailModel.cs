﻿using System;
using System.Collections.Generic;

namespace BingoMaster_Models
{
	public class BingoGameDetailModel
	{
		public string Name { get; set; }
		public DateTime Date { get; set; }
		public IEnumerable<PlayerModel> Players { get; set; }
		public int Size { get; set; }
	}
}