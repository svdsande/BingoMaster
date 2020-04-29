using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BingoMaster_Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BingoMaster_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BingoCardController : ControllerBase
    {
        #region Fields

        private readonly IBingoCardLogic _bingoCardLogic;

        #endregion

        public BingoCardController(IBingoCardLogic bingoCardLogic)
        {
            _bingoCardLogic = bingoCardLogic;
        }

        [HttpGet()]
        public BingoCard GenerateBingoCard(int numberOfCards, int rows, int columns)
        {
            return new BingoCard { Name = "Foo", Grids = _bingoCardLogic.GenerateCardGrids(numberOfCards, rows, columns) };
        }
    }
}