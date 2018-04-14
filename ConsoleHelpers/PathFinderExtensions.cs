using System;
using System.Collections.Generic;
using System.Linq;
using DrunkenMonk.Data.Enums;
using DrunkenMonk.Data.PathFinder;

namespace DrunkenMonk.ConsoleHelpers
{
	public static class PathFinderExtensions
	{
		public static List<Position> GetAvaibleNeighbors(
			this Position position,
			bool[,] field,
			List<Position> checkedList,
			Context ctx,
			AxisPriority? preferedAxis = null,
			Direction? direction = null)
		{
			List<Position> neighbors = new List<Position>(4);

			preferedAxis = preferedAxis ?? AxisPriority.None;

			try
			{
				#region Up

				if (position.Y > 0)
					if (!field[position.Y - 1, position.X]
							&& !checkedList.Any(x => x.X == position.X && x.Y == position.Y - 1))
						neighbors.Add(new Position
						{
							DistanceFromStart = position.DistanceFromStart + 1,
							X = position.X,
							Y = position.Y - 1,
							DirectionFromParent = Direction.Up
						});
				#endregion

				#region Down

				if (position.Y + 1 < field.GetLength(0))
					if (!field[position.Y + 1, position.X]
							&& !checkedList.Any(x => x.X == position.X && x.Y == position.Y + 1))
						if (preferedAxis == AxisPriority.PrioritizeY || direction == Direction.Down)
							neighbors.Insert(0, new Position
							{
								DistanceFromStart = position.DistanceFromStart + 1,
								X = position.X,
								Y = position.Y + 1,
								DirectionFromParent = Direction.Down
							});
						else
							neighbors.Add(new Position
							{
								DistanceFromStart = position.DistanceFromStart + 1,
								X = position.X,
								Y = position.Y + 1,
								DirectionFromParent = Direction.Down
							});

				#endregion

				#region Left
				if (position.X > 0)
					if (!field[position.Y, position.X - 1]
							&& !checkedList.Any(x => x.X == position.X - 1 && x.Y == position.Y))
						if (preferedAxis == AxisPriority.PrioritizeX || direction == Direction.Left)
							neighbors.Insert(0, new Position
							{
								DistanceFromStart = position.DistanceFromStart + 1,
								X = position.X - 1,
								Y = position.Y,
								DirectionFromParent = Direction.Left
							});
						else
							neighbors.Add(new Position
							{
								DistanceFromStart = position.DistanceFromStart + 1,
								X = position.X - 1,
								Y = position.Y,
								DirectionFromParent = Direction.Left
							});

				#endregion

				#region Right

				if (position.X + 1 < field.GetLength(1))
					if (!field[position.Y, position.X + 1]
							&& !checkedList.Any(x => x.X == position.X + 1 && x.Y == position.Y))
						if (preferedAxis == AxisPriority.PrioritizeX || direction == Direction.Right)
							neighbors.Insert(0, new Position
							{
								DistanceFromStart = position.DistanceFromStart + 1,
								X = position.X + 1,
								Y = position.Y,
								DirectionFromParent = Direction.Right
							});
						else
							neighbors.Add(new Position
							{
								DistanceFromStart = position.DistanceFromStart + 1,
								X = position.X + 1,
								Y = position.Y,
								DirectionFromParent = Direction.Right
							});

				#endregion
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}

			return neighbors.Select(pos => new Position
			{
				DistanceFromStart = pos.DistanceFromStart,
				X = pos.X,
				Y = pos.Y,
				RelativeToEnd = pos.GetRelativeDistace(ctx),
				DirectionFromParent = pos.DirectionFromParent
			}).ToList();
		}

		public static List<Position> GetAvaibleNeighbors(
			this Data.Base.Position position,
			bool[,] field,
			List<Position> checkedList,
			Context ctx)
		{
			return new Position(position).GetAvaibleNeighbors(field, new List<Position>(), ctx);
		}

		public static int GetAmountOfNeighbors(
			this Data.Base.Position pos,
			bool[,] field,
			List<Data.Base.Position> checkedList,
			Context ctx)
		{
			return new Position(pos).GetAvaibleNeighbors(field, new List<Position>(), ctx).Count;
		}

		public static int GetRelativeDistace(this Position position, Context ctx)
		{
			return Math.Abs(ctx.Target.X - position.X) + Math.Abs(ctx.Target.Y - position.Y);
		}
	}
}