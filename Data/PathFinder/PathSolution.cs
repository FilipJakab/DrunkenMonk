using System.Collections.Generic;
using System.Linq;

namespace DrunkenMonk.Data.PathFinder
{
	public class PathSolution
	{
		public List<Base.Position> Path { get; set; }

		public PathSolution(List<Position> path)
		{
			Path = new List<Base.Position>(path);
		}
	}
}