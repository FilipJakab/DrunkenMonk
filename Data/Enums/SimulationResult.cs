using DrunkenMonk.Data.Base;

namespace DrunkenMonk.Data.Enums
{
	public class SimulationResult
	{
		public bool HasSuccessfulyFinished { get; set; }

		public Position LastSafePosition { get; set; }

		public Position Obstacle { get; set; }
	}
}