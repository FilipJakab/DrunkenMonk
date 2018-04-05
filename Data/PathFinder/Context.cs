using System.Collections.Generic;

namespace DrunkenMonk.Data.PathFinder
{
	public class Context
	{
		public List<Position> OpenList { get; set; }

		public List<List<Position>> PosibleSolutions { get; set; } = new List<List<Position>>
		{
			new List<Position>()
		};

		public int CurrentSolutionIndex { get; set; }

		public Base.Position BasePosition { get; set; }

		public Base.Position Target { get; set; }

		/// <summary>
		/// True if obstacle is presented
		/// </summary>
		public bool[,] Field { get; set; }

		public bool? TraceFound { get; set; } = null;
	}
}