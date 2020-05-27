using System;

namespace BingoMaster_Models
{
    public class BingoCardCreationModel
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public bool IsCenterSquareFree { get; set; }
        public int Amount { get; set; }
    }
}
