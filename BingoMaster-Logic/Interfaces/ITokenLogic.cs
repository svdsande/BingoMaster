using BingoMaster_Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoMaster_Logic.Interfaces
{
    public interface ITokenLogic
    {
        string GenerateToken(User user);
    }
}
