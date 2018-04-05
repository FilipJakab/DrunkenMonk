using System;
using System.Collections.Generic;
using System.Linq;
using DrunkenMonk.Data.PathFinder;

namespace DrunkenMonk.ConsoleHelpers
{
	public static class PathFinderExtensions
	{
		public static List<Position> GetAvaibleNeighbor(this Position position, Context ctx)
		{
			#region Old

			//List<Position> neighbors = new List<Position>(4)
			//{
			//	// Up
			//	new Position
			//	{
			//		DistanceFromStart = position.DistanceFromStart + 1,
			//		X = position.X,
			//		Y = position.Y - 1
			//	},
			//	// Left
			//	new Position
			//	{
			//		DistanceFromStart = position.DistanceFromStart + 1,
			//		X = position.X - 1,
			//		Y = position.Y
			//	},
			//	// Right
			//	new Position
			//	{
			//		DistanceFromStart = position.DistanceFromStart + 1,
			//		X = position.X + 1,
			//		Y = position.Y
			//	},
			//	// Down
			//	new Position
			//	{
			//		DistanceFromStart = position.DistanceFromStart + 1,
			//		X = position.X,
			//		Y = position.Y + 1
			//	}
			//};

			#endregion

			List<Position> neighbors = new List<Position>(4);

			#region Up

			if (!ctx.Field[position.Y >= 1 ? position.Y - 1 : 0, position.X])
				neighbors.Add(new Position
				{
					DistanceFromStart = position.DistanceFromStart + 1,
					X = position.X,
					Y = position.Y >= 1 ? position.Y - 1 : 0
				});

			#endregion

			#region Down

			if (
				!ctx.Field[position.Y + 1 < ctx.Field.GetLength(0) ? position.Y + 1 : ctx.Field.GetLength(0) - 1,
					position.X])
				neighbors.Add(new Position
				{
					DistanceFromStart = position.DistanceFromStart + 1,
					X = position.X,
					Y = position.Y + 1 < ctx.Field.GetLength(0) ? position.Y + 1 : ctx.Field.GetLength(0) - 1
				});

			#endregion

			#region Left

			if (!ctx.Field[position.Y, position.X > 0 ? position.X - 1 : 0])
				neighbors.Add(new Position
				{
					DistanceFromStart = position.DistanceFromStart + 1,
					X = position.X > 0 ? position.X - 1 : 0,
					Y = position.Y
				});

			#endregion

			#region Right

			if (
				!ctx.Field[position.Y,
					position.X < ctx.Field.GetLength(1) ? position.X + 1 : ctx.Field.GetLength(1) - 1])
				neighbors.Add(new Position
				{
					DistanceFromStart = position.DistanceFromStart + 1,
					X = position.X < ctx.Field.GetLength(1) ? position.X + 1 : ctx.Field.GetLength(1) - 1,
					Y = position.Y
				});

			#endregion

			return neighbors.Select(pos => new Position
			{
				DistanceFromStart = pos.DistanceFromStart,
				X = pos.X,
				Y = pos.Y,
				RelativeToEnd = pos.GetRelativeDistace(ctx)
			}).ToList();
		}

		public static int GetRelativeDistace(this Position position, Context ctx)
		{
			return Math.Abs(ctx.Target.X - position.X) + Math.Abs(ctx.Target.Y - position.Y);
		}

		public static List<Position> CopyPath(this List<Position> list) => list.Select(x => x).ToList();
	}
}