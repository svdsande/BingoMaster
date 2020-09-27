using BingoMaster_Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic
{
    public interface IBingoCardLogic
    {
        IEnumerable<BingoCardModel> GenerateBingoCards(BingoCardCreationModel bingoCard);
    }
}
