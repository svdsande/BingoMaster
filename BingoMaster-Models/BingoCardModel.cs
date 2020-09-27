using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BingoMaster_Models
{
    public class BingoCardModel
    {
        public string Name { get; set; }
        public int?[][] Grid { get; set; }
    }
}
