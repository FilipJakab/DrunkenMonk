using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DrunkenMonk.ConsoleHelpers;
using DrunkenMonk.Data.Base;
using DrunkenMonk.Data.Constants;
using DrunkenMonk.Data.Enums;
using DrunkenMonk.Data.PathFinder;
using Position = DrunkenMonk.Data.PathFinder.Position;

namespace DrunkenMonk.Providers
{
	public class PathFinderProvider
	{
		public Context CreateContext(Data.Base.Position @base, Data.Base.Position target, bool[,] field)
		{
			Context ctx = new Context
			{
				Field = field,
				Target = target,
				BasePosition = @base,
				ClosedList = new List<Position>(),
				OpenList = new List<Position>()
			};

			return ctx;
		}

		public bool[,] DeadFill(
			bool[,] field,
			Canvas canvas,
			PaintBrush brush,
			List<Data.Base.Position> excludes,
			Context ctx)
		{
			void CheckNeighbors(Position basePosition, List<Position> checkedListInternal)
			{
				if (excludes.Any(x => x.X == basePosition.X && x.Y == basePosition.Y))
					return;

				List<Position> neighbors = basePosition.GetAvaibleNeighbors(field, checkedListInternal, ctx);

				if (neighbors.Count > 1)
					return;

				//brush.Render(canvas, new Data.Base.Position(basePosition.X, basePosition.Y), CharMap.LightTrail);

				if (neighbors.Count == 0)
					return;

				checkedListInternal.Add(neighbors.First());
				CheckNeighbors(neighbors.First(), checkedListInternal);
			}

			for (int i = 0; i < 2; i++)
			{
				for (int y = 0; y < field.GetLength(0); y++)
				{
					for (int x = 0; x < field.GetLength(1); x++)
					{
						//if (field[y, x]) continue;

						CheckNeighbors(new Position(x, y), new List<Position>());
					}
				}
			}

			return field;
		}

		//public List<PathSolution> FindPaths(Context ctx)
		//{
		//	List<PathSolution> solutions = new List<PathSolution>();

		//	void Iteration(
		//		Position basePosition,
		//		List<Position> closedList,
		//		List<Position> openList,
		//		AxisPriority priority,
		//		AxisPriority? helperPriority = null)
		//	{
		//		if (priority == AxisPriority.PrioritizeBoth)
		//			helperPriority = (helperPriority ?? AxisPriority.PrioritizeX) == AxisPriority.PrioritizeX
		//				? AxisPriority.PrioritizeY
		//				: AxisPriority.PrioritizeX;
		//		else
		//			helperPriority = priority;

		//		List<Position> neighbors = basePosition.GetAvaibleNeighbors(ctx.Field, openList, ctx, helperPriority);

		//		if (neighbors.Count == 0)
		//			return;

		//		openList.AddRange(neighbors);

		//		if (neighbors.Any(x => x.RelativeToEnd == 0))
		//		{
		//			closedList.Add(neighbors.First(x => x.RelativeToEnd == 0));
		//			solutions.Add(new PathSolution(closedList));
		//			return;
		//		}

		//		int minimal = neighbors.Min(x => x.RelativeToEnd);
		//		neighbors
		//			.Where(x => x.RelativeToEnd == minimal)
		//			.ToList()
		//			.ForEach(x =>
		//			{
		//				Iteration(
		//					x, new List<Position>(closedList) {x},
		//					new List<Position>(openList), helperPriority.Value);
		//			});
		//	}

		//	Iteration(new Position(ctx.BasePosition),
		//		new List<Position>(), new List<Position>(),
		//		AxisPriority.PrioritizeY);

		//	//Parallel.Invoke(() =>
		//	//	Iteration(new Position(ctx.BasePosition),
		//	//		new List<Position>(), new List<Position>(),
		//	//		AxisPriority.PrioritizeY),
		//	//	() =>
		//	//		Iteration(new Position(ctx.BasePosition),
		//	//			new List<Position>(), new List<Position>(),
		//	//			AxisPriority.PrioritizeX),
		//	//	() =>
		//	//		Iteration(new Position(ctx.BasePosition),
		//	//			new List<Position>(), new List<Position>(),
		//	//			AxisPriority.PrioritizeBoth));

		//	return new List<PathSolution>();
		//}

		/// <summary>
		/// Method implementing A* path finding algorithm.
		/// May not find the 'shortest' path...
		/// </summary>
		/// <exception cref="StackOverflowException">Uses recursion</exception>
		/// <param name="ctx"></param>
		/// <returns></returns>
		public PathSolution FindPath(Context ctx)
		{
			List<Position> solution = new List<Position>();
			bool pathFound = false;

			void Iteration(List<Position> closedList, List<Position> openList, Position basePosition)
			{
				if (pathFound)
					return;

				List<Position> neighbors = basePosition.GetAvaibleNeighbors(
					ctx.Field, openList,
					ctx, direction: basePosition.DirectionFromParent);

				if (neighbors.Count == 0)
					return;

				if (neighbors.Any(x => x.RelativeToEnd == 0))
				{
					pathFound = true;
					closedList.Add(neighbors.First(x => x.RelativeToEnd == 0));

					solution.AddRange(closedList);

					return;
				}

				openList.AddRange(neighbors);
				List<Position> closedListCopy = new List<Position>(closedList);

				int minimalDistance = neighbors.Min(x => x.RelativeToEnd);

				void LoopIterationNeighbors(Position basePos)
				{
					if (pathFound)
						return;

					closedListCopy.Add(basePos);

					Iteration(closedListCopy, openList, basePos);

					if (!pathFound)
						closedListCopy.Remove(basePos);
				}

				neighbors.Where(x => x.RelativeToEnd == minimalDistance)
					.ToList()
					.ForEach(LoopIterationNeighbors);

				if (pathFound)
				{
					return;
				}

				neighbors.Where(x => x.RelativeToEnd != minimalDistance)
					.ToList()
					.ForEach(LoopIterationNeighbors);
			}

			Iteration(ctx.ClosedList, ctx.OpenList, new Position(ctx.BasePosition));

			return pathFound
				? new PathSolution(solution)
				: new PathSolution(new List<Position>());
		}

		///// <summary>
		///// method implementing simple A* path-finding algorithm
		///// </summary>
		///// <returns></returns>
		//public List<PathSolution> FindPaths(
		//	bool[,] field,
		//	List<Data.Base.Position> openList,
		//	List<Position> closedList,
		//	Data.Base.Position basePosition,
		//	Data.Base.Position targetPosition)
		//{
		//	void iteration()
		//	{

		//	}
		//}
	}
}