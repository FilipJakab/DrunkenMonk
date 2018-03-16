namespace DrunkenMonk.Data
{
	public class Position
	{
		public int X { get; set; }

		public int Y { get; set; }

		public Position()
		{
		}

		public static Position Copy(Position position)
		{
			return new Position(position.X, position.Y);
		}

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static bool Compare(Position pos1, Position pos2) => pos1.X == pos2.X && pos1.Y == pos2.Y;
	}
}