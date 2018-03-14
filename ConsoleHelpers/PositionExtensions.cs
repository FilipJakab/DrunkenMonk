using System;
using System.Collections.Generic;
using System.Linq;
using DrunkenMonk.Data;
using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.ConsoleHelpers
{
	public static class PositionExtensions
	{
		// TODO: Fisnish Converting absolute values to relative values based on canvas
		public static IEnumerable<Position> ToRelative(this IEnumerable<Position> positions, Canvas canvas)
		{
			throw new NotImplementedException($"{nameof(ToRelative)} is not Implemented yet.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="obstacles"></param>
		/// <returns>True if position is equal to atleast one obstacle in obstacles</returns>
		public static bool PredictCollision(this Position position, IEnumerable<Position> obstacles)
		{
			return obstacles.Any(obstacle => obstacle.X == position.X && obstacle.Y == position.Y);
		}

		/// <summary>
		/// Simulates Collision of players Position which is currently on any obstacle (position is already predicted)
		/// </summary>
		/// <param name="position"></param>
		/// <param name="oldPosition"></param>
		/// <param name="oldDirection"></param>
		/// <returns></returns>
		public static Simulation SimulateCollision(this Position position, Position oldPosition, Direction oldDirection)
		{
			/**
			 * TODO: Refactor this method
			 * Make BasePosition pointing to obstacle, not player
			 */

			Position newPosition = new Position(position.X, position.Y);

			Random random = new Random(DateTime.Now.Millisecond);

			// Generates 20% chance int 1-5
			int punchChance = random.Next(1, 6);

			return new Simulation
			{
				BasePosition = oldPosition,
				Direction = oldDirection.Reverse(),
				Difference = punchChance == 1
					? random.Next(3, 5)	// Punch
					: 1,								// No punch
				RenderCharacter = Player.BodyCharacter
			};
		}
	}
}