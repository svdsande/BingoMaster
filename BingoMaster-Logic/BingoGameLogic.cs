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
	public class BingoGameLogic : IBingoGameLogic
	{
		#region Fields

		private readonly IBingoCardLogic _bingoCardLogic;
		private readonly IBingoNumberLogic _bingoNumberLogic;
		private readonly BingoMasterDbContext _context;
		private readonly IMapper _mapper;

		#endregion

		public BingoGameLogic(IBingoCardLogic bingoCardLogic, IBingoNumberLogic bingoNumberLogic, BingoMasterDbContext context, IMapper mapper)
		{
			_bingoCardLogic = bingoCardLogic;
			_bingoNumberLogic = bingoNumberLogic;
			_context = context;
			_mapper = mapper;
		}

		public BingoGameModel CreateNewGame(BingoGameDetailModel gameDetailModel)
		{
			if (gameDetailModel == null || string.IsNullOrWhiteSpace(gameDetailModel.Name) || gameDetailModel.MaximumAmountOfPlayers <= 0 || (gameDetailModel.Players?.Count() ?? 0) > gameDetailModel.MaximumAmountOfPlayers || gameDetailModel.Size <= 0)
			{
				throw new ArgumentException("No name for the game provided or invalid number of players or grid size");
			}

			if (gameDetailModel.Date.Date.CompareTo(DateTime.Now.Date) < 0)
			{
				throw new ArgumentOutOfRangeException("Date for the game cannot be in the past");
			}

			var creator = _context.Players.Find(gameDetailModel.CreatorId);

			if (creator == null)
			{
				throw new KeyNotFoundException("Creator does not exists");
			}

			var game = _mapper.Map<Game>(gameDetailModel);
			game.Creator = creator;

			AssignPlayersToGame(gameDetailModel, creator, game);

			_context.Games.Add(game);
			_context.SaveChanges();

			return _mapper.Map<BingoGameModel>(game);
		}

		public BingoGameModel PlayRound(IEnumerable<PlayerGameModel> players, int[] drawnNumbers)
		{
			if (players.Count() <= 0 || drawnNumbers.Length <= 0)
			{
				throw new ArgumentException("Invalid number of players or drawn numbers");
			}

			var nextNumber = _bingoNumberLogic.GetNextNumber();
			var numbers = drawnNumbers.Select(number => (int?)number).ToList();
			numbers.Add(nextNumber);

			foreach (var player in players)
			{
				player.IsHorizontalLineDone = HorizontalLineDone(player.BingoCard.Grid, numbers);
				player.IsFullCardDone = FullCardDone(player.BingoCard.Grid, numbers);
			}

			return new BingoGameModel()
			{
				DrawnNumber = nextNumber,
				Players = players
			};
		}

		private bool HorizontalLineDone(int?[][] grid, List<int?> drawnNumbers)
		{
			foreach (var row in grid)
			{
				var cleanNumbers = row.Where(number => number != null);
				var intersection = cleanNumbers.Intersect(drawnNumbers);

				if (intersection.Count() == cleanNumbers.Count())
				{
					return true;
				}
			}

			return false;
		}

		private bool FullCardDone(int?[][] grid, List<int?> drawnNumbers)
		{
			var gridNumbers = grid.SelectMany(row => row).Where(number => number != null).ToArray();
			var intersection = gridNumbers.Intersect(drawnNumbers);

			if (intersection.Count() == gridNumbers.Count())
			{
				return true;
			}

			return false;
		}

		private void AssignPlayersToGame(BingoGameDetailModel gameDetailModel, Player creator, Game game)
		{
			if (gameDetailModel.Players?.Any() == true)
			{
				var playerIds = gameDetailModel.Players.Select(player => player.Id);
				var players = _context.Players.Where(player => playerIds.Contains(player.Id)).ToArray();

				game.GamePlayers = players.Select(player => new GamePlayer { Game = game, Player = player }).ToList();
			}

			if (game.GamePlayers?.Any() == true)
			{
				game.GamePlayers.Add(new GamePlayer { Game = game, Player = creator });
			} 
			else
			{
				game.GamePlayers = new GamePlayer[] { new GamePlayer { Game = game, Player = creator } };
			}

		}
	}
}
