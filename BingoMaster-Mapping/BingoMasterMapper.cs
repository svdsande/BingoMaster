using AutoMapper;
using BingoMaster_Entities;
using BingoMaster_Models;
using BingoMaster_Models.Player;
using BingoMaster_Models.User;
using System;
using System.Linq;

namespace BingoMaster_Mapping
{
	public class BingoMasterMapper : Profile
	{
		public BingoMasterMapper()
		{
			// User
			CreateMap<User, UserModel>()
				.ForMember(dest => dest.PlayerName, opt => opt.MapFrom(src => src.Player.Name));
			CreateMap<User, AuthenticatedUserModel>()
				.ForMember(dest => dest.PlayerName, opt => opt.MapFrom(src => src.Player.Name))
				.ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Player.Id));

			// Game
			CreateMap<Game, BingoGameDetailModel>()
				.ForMember(dest => dest.IsCenterSquareFree, opt => opt.MapFrom(src => src.CenterSquareFree))
				.ForMember(dest => dest.IsPrivateGame, opt => opt.MapFrom(src => src.Private))
				.ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Grid))
				.ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.GamePlayers.Select(gamePlayer => gamePlayer.Player)))
				.ReverseMap();
			CreateMap<Game, BingoGameModel>()
				.ForMember(dest => dest.Players, opt => opt.MapFrom(src => src.GamePlayers.Select(gamePlayer => gamePlayer.Player)));

			// Player
			CreateMap<Player, PlayerGameModel>();
			CreateMap<Player, PlayerModel>().ReverseMap();

			// GamePlayer
			CreateMap<GamePlayer, PlayerGameModel>();
		}
	}
}
