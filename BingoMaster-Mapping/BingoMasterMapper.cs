using AutoMapper;
using BingoMaster_Entities;
using BingoMaster_Models;
using BingoMaster_Models.Player;
using BingoMaster_Models.User;
using System;

namespace BingoMaster_Mapping
{
	public class BingoMasterMapper : Profile
	{
		public BingoMasterMapper()
		{
			// User
			CreateMap<User, UserModel>();
			CreateMap<User, AuthenticatedUserModel>()
				.ForMember(dest => dest.PlayerId, opt => opt.MapFrom(src => src.Player.Id));

			// Game
			CreateMap<Game, BingoGameDetailModel>();

			// Player
			CreateMap<Player, PlayerGameModel>();
			CreateMap<Player, PlayerModel>().ReverseMap();
		}
	}
}
