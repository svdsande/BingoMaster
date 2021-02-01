namespace BingoMaster_Models
{
	public class PlayerGameModel
	{
		public string Name { get; set; }
		public BingoCardModel BingoCard { get; set; }
		public bool IsHorizontalLineDone { get; set; }
		public bool IsVerticalLineDone { get; set; }
		public bool IsFullCardDone { get; set; }
	}
}
