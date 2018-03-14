using DrunkenMonk.Data.Base;
using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.Data
{
	public class Simulation
	{
		public Position BasePosition { get; set; }

		public Position LastSafePosition { get; set; }

		public Direction Direction { get; set; }

		public int Difference { get; set; }

		public char RenderCharacter { get; set; }
	}
}