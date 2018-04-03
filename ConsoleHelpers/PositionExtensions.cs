using System;
using System.Collections.Generic;
using System.Linq;
using DrunkenMonk.Data;
using DrunkenMonk.Data.Base;
using DrunkenMonk.Data.Enums;
using NLog;

namespace DrunkenMonk.ConsoleHelpers
{
	public static class PositionExtensions
	{
		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		// TODO: Fisnish Converting absolute values to relative values based on canvas
		public static IEnumerable<Position> ToRelative(this IEnumerable<Position> positions, Canvas canvas)
		{
			logger.Trace($"{nameof(ToRelative)} method called");

			throw new NotImplementedException($"{nameof(ToRelative)} is not Implemented yet.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="position"></param>
		/// <param name="obstacles"></param>
		/// <returns>True if obstaclePosition is equal to atleast one obstacle in obstacles</returns>
		public static bool PredictCollision(this Position position, IEnumerable<Position> obstacles)
		{
			logger.Trace($"{nameof(PredictCollision)} method called");

			return obstacles.Any(obstacle => obstacle.X == position.X && obstacle.Y == position.Y);
		}

		/// <summary>
		/// Simulates Collision of players Position which is currently on any obstacle (obstaclePosition is already predicted)
		/// </summary>
		/// <param name="position">new obstaclePosition (obstacle if collision is unavoidable)</param>
		/// <param name="oldPosition">Position of player</param>
		/// <param name="oldDirection"></param>
		/// <param name="minSteps"></param>
		/// <param name="maxSteps"></param>
		/// <returns></returns>
		public static Simulation SimulateCollision(
			this Position position,
			Position oldPosition,
			Direction oldDirection,
			int minSteps, int maxSteps)
		{
			logger.Trace($"{nameof(SimulateCollision)} method called");

			Random random = new Random(DateTime.Now.Millisecond);

			// Generates 20%
			int punchChance = random.Next(1, 5);
			logger.Debug($"Chance of punch is {1 / 5f} punched ? {(punchChance == 1 ? "Yes" : "No")}");

			return new Simulation
			{
				BasePosition = position,
				Direction = oldDirection.Reverse(),
				Difference = punchChance == 1
					? random.Next(minSteps, maxSteps + 1) // Punch
					: 1, // No punch
				LastSafePosition = oldPosition,
				RenderCharacter = Player.BodyCharacter
			};
		}

		public static Simulation SimulateTrip(
			this Position obstaclePosition,
			Position playerPosition,
			Direction playerDirection,
			int minSteps, int maxSteps)
		{
			logger.Trace($"{nameof(SimulateTrip)} method called");

			Random random = new Random(DateTime.Now.Millisecond);

			return new Simulation
			{
				BasePosition = playerPosition,
				Direction = playerDirection,
				Difference = random.Next(minSteps, maxSteps + 1),
				LastSafePosition = obstaclePosition, // collision was already checked
				RenderCharacter = Player.BodyCharacter
			};
		}

		public static bool Compare(this Position pos1, Position pos2) => pos1.X == pos2.X && pos1.Y == pos2.Y;
	}
}