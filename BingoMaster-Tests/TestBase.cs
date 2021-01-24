using AutoMapper;
using BingoMaster_Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Tests
{
	public class TestBase
	{
		public IMapper _mapper;

		public TestBase()
		{
			InitializeMapper();
		}

		private void InitializeMapper()
		{
			var bingoMasterMapper = new BingoMasterMapper();
			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(bingoMasterMapper));

			_mapper = new Mapper(configuration);
		}
	}
}
