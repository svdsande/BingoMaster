using AutoMapper;
using BingoMaster_Entities;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using BingoMaster_Models.Player;
using Microsoft.EntityFrameworkCore;
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

			var games = _context.Games
				.Include(game => game.GamePlayers)
				.ThenInclude(gamePlayer => gamePlayer.Player)
				.Where(game => game.GamePlayers.Any(gamePlayer => gamePlayer.PlayerId == id))
				.ToArray();
			
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

		public bool PlayerNameUnique(string playerName)
		{
			if (string.IsNullOrWhiteSpace(playerName))
			{
				throw new ArgumentException("No player name provided");
			}

			var player = _context.Players.FirstOrDefault(player => player.Name == playerName);

			if (player != null)
			{
				return false;
			}

			return true;
		}

		public IEnumerable<PlayerModel> GetAllPlayers()
		{
			var players = _context.Players.ToArray();

			return _mapper.Map<IEnumerable<Player>, IEnumerable<PlayerModel>>(players);
		}
	}
}
