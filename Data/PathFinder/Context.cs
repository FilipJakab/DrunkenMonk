using System.Collections.Generic;
using System.Linq;

namespace DrunkenMonk.Data.PathFinder
{
	public class Context
	{
		public List<Position> OpenList { get; set; }

		public List<Position> ClosedList { get; set; }

		public Base.Position BasePosition { get; set; }

		public Base.Position Target { get; set; }

		/// <summary>
		/// True if obstacle is presented
		/// </summary>
		public bool[,] Field { get; set; }

		public List<Position> Solution { get; set; } = new List<Position>();
	}
}