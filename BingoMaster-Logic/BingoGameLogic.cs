using AutoMapper;
using BingoMaster_Entities;
using BingoMaster_Logic.Interfaces;
using BingoMaster_Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoMaster_Logic
{
	public class BingoGameLogic : IBingoGameLogic
	{
		#region Fields

		private readonly IBingoNumberLogic _bingoNumberLogic;
		private readonly BingoMasterDbContext _context;
		private readonly IMapper _mapper;

		#endregion

		public BingoGameLogic(IBingoNumberLogic bingoNumberLogic, BingoMasterDbContext context, IMapper mapper)
		{
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
				return null;
			}

			var game = _mapper.Map<Game>(gameDetailModel);
			game.Creator = creator;

			AssignPlayersToGame(gameDetailModel, game);

			_context.Games.Add(game);
			_context.SaveChanges();

			return _mapper.Map<BingoGameModel>(game);
		}

		public BingoGameModel PlayRound(IEnumerable<PlayerGameModel> players, int[] drawnNumbers)
        {
            if (!players.Any() || !drawnNumbers.Any())
            {
                throw new ArgumentException("Invalid number of players or drawn numbers");
            }

            var nextNumber = _bingoNumberLogic.GetNextNumber();
            var numbers = drawnNumbers.Select(number => (int?)number).ToList();
            numbers.Add(nextNumber);

            CheckBingoCards(players, numbers);

            return new BingoGameModel()
            {
                DrawnNumber = nextNumber,
                Players = players
            };
        }

        public IEnumerable<BingoGameDetailModel> GetAllPublicBingoGames()
		{
			var games = _context.Games
				.Include(game => game.GamePlayers)
				.ThenInclude(gamePlayer => gamePlayer.Player)
				.Where(game => !game.Private)
				.ToArray();

			return _mapper.Map<IEnumerable<Game>, IEnumerable<BingoGameDetailModel>>(games);
		}

		public void JoinGame(Guid gameId, Guid playerId)
        {
			var game = TryGetGameWithPlayers(gameId, playerId);

			if (!IsPlayerAlreadyGameParticipant(playerId, game) && !IsMaximumAmountOfGamePlayersReached(game))
            {
                var player = _context.Players.SingleOrDefault(player => player.Id == playerId);
                game.GamePlayers.Add(new GamePlayer { Game = game, Player = player });
                _context.SaveChanges();
            }
        }

        public void LeaveGame(Guid gameId, Guid playerId)
        {
            var game = TryGetGameWithPlayers(gameId, playerId);

            if (IsPlayerAlreadyGameParticipant(playerId, game))
            {
                var existingGamePlayer = game.GamePlayers.SingleOrDefault(gamePlayer => gamePlayer.PlayerId == playerId);
                game.GamePlayers.Remove(existingGamePlayer);
                _context.SaveChanges();
            }
        }

		private void CheckBingoCards(IEnumerable<PlayerGameModel> players, List<int?> numbers)
		{
			foreach (var player in players)
			{
				player.IsHorizontalLineDone = HorizontalLineDone(player.BingoCard.Grid, numbers);
				player.IsFullCardDone = FullCardDone(player.BingoCard.Grid, numbers);
			}
		}

		private Game TryGetGameWithPlayers(Guid gameId, Guid playerId)
        {
            if (gameId == Guid.Empty || playerId == Guid.Empty)
            {
                throw new ArgumentException("No game id or player id provided");
            }

            return GetGameIncludingGamePlayers(gameId);
        }

        private bool IsPlayerAlreadyGameParticipant(Guid playerId, Game game)
        {
            return game?.GamePlayers?.Any(gamePlayer => gamePlayer.PlayerId == playerId) ?? false;
        }

		private bool IsMaximumAmountOfGamePlayersReached(Game game)
		{
			return game?.GamePlayers?.Count() == game?.MaximumAmountOfPlayers;
		}

		private Game GetGameIncludingGamePlayers(Guid gameId)
		{
			return _context.Games
				.Include(game => game.GamePlayers)
				.SingleOrDefault(game => game.Id == gameId);
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

		private void AssignPlayersToGame(BingoGameDetailModel gameDetailModel, Game game)
		{
			if (gameDetailModel.Players?.Any() == true)
			{
				var playerIds = gameDetailModel.Players.Select(player => player.Id);
				var players = _context.Players.Where(player => playerIds.Contains(player.Id)).ToArray();

				game.GamePlayers = players.Select(player => new GamePlayer { Game = game, Player = player }).ToList();
			}
		}
	}
}
