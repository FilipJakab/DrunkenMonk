using System;
using DrunkenMonk.Data.Exceptions;

namespace DrunkenMonk.Data.Base
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

		public static T Copy<T>(Position position) where T: PathFinder.Position
		{
			if (typeof(T) != typeof(PathFinder.Position))
				throw new InvalidTypeException();

			return (T) new PathFinder.Position
			{
				DistanceFromStart = 0,
				RelativeToEnd = 0,
				X = position.X,
				Y = position.Y
			};
		}

		public Position(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static bool Compare(Position pos1, Position pos2) => pos1.X == pos2.X && pos1.Y == pos2.Y;
	}
}