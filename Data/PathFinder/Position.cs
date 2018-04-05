namespace DrunkenMonk.Data.PathFinder
{
	public class Position : Base.Position
	{
		public int Score => DistanceFromStart + RelativeToEnd;

		public int RelativeToEnd { get; set; }

		public int DistanceFromStart { get; set; }
	}
}