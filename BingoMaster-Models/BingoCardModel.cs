using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BingoMaster_API
{
    public class BingoCardModel
    {
        public string Name { get; set; }
        public int[,,] Grids { get; set; }
    }
}
