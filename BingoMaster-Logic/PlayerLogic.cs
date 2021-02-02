﻿using AutoMapper;
using BingoMaster_Entities;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using BingoMaster_Models.Player;
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

		public PlayerModel GetPlayerById(Guid id)
		{
			var player = _context.Players.Find(id);

			return _mapper.Map<PlayerModel>(player);
		}

		public void Update(PlayerModel playerModel)
		{
			var player = _context.Players.FirstOrDefault(player => player.Id == playerModel.Id);

			if (player == null)
			{
				throw new KeyNotFoundException("Entity does not exists");
			}

			_context.Entry(player).CurrentValues.SetValues(playerModel);
			_context.SaveChanges();
		}
	}
}
