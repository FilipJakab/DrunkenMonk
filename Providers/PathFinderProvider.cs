using System.Collections.Generic;
using System.Linq;
using DrunkenMonk.ConsoleHelpers;
using DrunkenMonk.Data.PathFinder;

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
				PosibleSolutions = new List<List<Position>>
				{
					new List<Position>
					{
						Data.Base.Position.Copy<Position>(Data.Base.Position.Copy<Position>(@base))
					}
				}
			};

			return ctx;
		}

		public void FindTrace(Context ctx)
		{
			void processOtherSolutions()
			{
				//ctx.PosibleSolutions[]
				//	.Add(neighbors.First(x => x.Score == minimalScore));
			}

			void copyPreviousTrail(List<Position> trail)
			{

			}

			void checkNeighbors(List<Position> neighbors)
			{
				int minimalScore = neighbors.Min(pos => pos.Score),
					duplicates = neighbors.Where(pos => pos.Score == minimalScore).ToList().Count;

				ctx.PosibleSolutions[ctx.CurrentSolutionIndex].Add(neighbors.First(x => x.Score == minimalScore));

				if (duplicates == 0) return;

				for (int i = 0; i < duplicates; i++)
					ctx.PosibleSolutions.Add(ctx.PosibleSolutions.Last().CopyPath());
			}
		}
	}
}