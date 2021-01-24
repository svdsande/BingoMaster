using BingoMaster_Entities;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic
{
	public class PlayerLogic : IPlayerLogic
	{
		#region Fields

		private readonly BingoMasterDbContext _context;

		#endregion

		public PlayerLogic(BingoMasterDbContext context)
		{
			_context = context;
		}
	}
}
