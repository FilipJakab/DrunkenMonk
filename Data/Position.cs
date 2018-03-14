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
		//public Position(Position position)
		//{
		//	X = position.X;
		//	Y = position.Y;
		//}

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}