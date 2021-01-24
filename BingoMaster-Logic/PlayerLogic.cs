using AutoMapper;
using BingoMaster_Entities;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoMaster_Logic
{
	public class PlayerLogic : IPlayerLogic
	{
		#region Fields

		private readonly BingoMasterDbContext _context;
		private readonly IMapper _mapper;

		#endregion

		public PlayerLogic(BingoMasterDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public IEnumerable<BingoGameDetailModel> GetGamesForPlayer(Guid id)
		{
			if (id == Guid.Empty)
			{
				throw new ArgumentException("No player id provided");
			}

			var games = _context.GamePlayers.Where(gamePlayer => gamePlayer.PlayerId == id).Select(gamePlayer => gamePlayer.Game).ToArray();

			return _mapper.Map<IEnumerable<Game>, IEnumerable<BingoGameDetailModel>>(games);
		}
	}
}
