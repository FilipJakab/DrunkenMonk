using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.Data.PathFinder
{
	public class Position : Base.Position
	{
		public int Score => DistanceFromStart + RelativeToEnd;

		public int RelativeToEnd { get; set; }

		public int DistanceFromStart { get; set; }

		public Direction DirectionFromParent { get; set; }

		public Position(Base.Position position)
		{
			X = position.X;
			Y = position.Y;
		}

		public Position(int x, int y) : base(x, y)
		{ }

		public Position()
		{
		}
	}
}