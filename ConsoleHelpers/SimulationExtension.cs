using System;
using DrunkenMonk.Data;
using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.ConsoleHelpers
{
	public static class SimulationExtension
	{
		public static Position GetFinalPosition(this Simulation simulation)
		{
			switch (simulation.Direction)
			{
				case Direction.Down:
					{
						return new Position(simulation.BasePosition.X, simulation.BasePosition.Y + simulation.Difference);
					}
				case Direction.Left:
					{
						return new Position(simulation.BasePosition.X - simulation.Difference, simulation.BasePosition.Y);
					}
				case Direction.Right:
					{
						return new Position(simulation.BasePosition.X + simulation.Difference, simulation.BasePosition.Y);
					}
				case Direction.Up:
					{
						return new Position(simulation.BasePosition.X, simulation.BasePosition.Y - simulation.Difference);
					}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(simulation.Direction));
				}
			}
		}
	}
}