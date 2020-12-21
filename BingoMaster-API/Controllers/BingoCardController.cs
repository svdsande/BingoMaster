using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BingoMaster_Logic;
using BingoMaster_Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

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

        [HttpPost]
        [SwaggerResponse(HttpStatusCode.OK, typeof(IEnumerable<BingoCardModel>))]
        [SwaggerResponse(HttpStatusCode.BadRequest, typeof(ErrorModel))]
        public ActionResult GenerateBingoCards([FromBody] BingoCardCreationModel bingoCardModel)
        {
            if (bingoCardModel == null)
            {
                return BadRequest("Invalid form data provided");
            }

            var bingoCards = _bingoCardLogic.GenerateBingoCards(bingoCardModel);

            return Ok(bingoCards);
        }
    }
}