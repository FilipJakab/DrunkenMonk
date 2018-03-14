using System;
using System.Collections.Generic;
using System.Threading;
using DrunkenMonk.Data;
using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.ConsoleHelpers
{
	public static class CanvasExtensions
	{
		/// <summary>
		/// Sets the Cursor position relatively to canvas
		/// </summary>
		/// <param name="canvas">Map used as relative reference</param>
		/// <param name="left">X Coord</param>
		/// <param name="top">Y Coord</param>
		/// <param name="validateInput"></param>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public static void SetCursorPosition(this Canvas canvas, int left, int top, bool validateInput = true)
		{
			// Substract walls
			if (validateInput)
			{
				if (left >= canvas.Width - 2 || left < 0)
					throw new ArgumentOutOfRangeException(nameof(left), $"Parameter is out of Map; width: {canvas.Width}, left: {left}");
				if (top >= canvas.Height - 2 || top < 0)
					throw new ArgumentOutOfRangeException(nameof(top), $"Parameter is out of Map; height: {canvas.Height}, top: {top}");
			}

			// + 1 because top-left border is at StartX and StartY
			Console.SetCursorPosition(
				left + canvas.StartX + 1,
				top + canvas.StartY + 1);
		}

		public static void /*IEnumerable<Position>*/ ExecuteSimulation(
			this Canvas canvas,
			Simulation simulation,
			Func<Position, bool> validate)
		{
			PaintBrush brush = new PaintBrush();

			brush.Derender(canvas, simulation.BasePosition);

			Position tmpPosition = new Position(simulation.BasePosition.X, simulation.BasePosition.Y);

			void SimulationIteration(Action<Position> modification)
			{
				simulation.LastSuccessfulPosition = tmpPosition;

				brush.Derender(canvas, tmpPosition);

				modification(tmpPosition);

				if (!validate(tmpPosition))
					return;

				brush.Render(canvas, tmpPosition, simulation.RenderCharacter);

				// TODO: Move delayTime to constants / Appconfig
				Thread.Sleep(350);
			}

			switch (simulation.Direction)
			{
				case Direction.Down:
					{
						for (int i = 0; i < simulation.Difference; i++)
						{
							SimulationIteration(position => position.Y++);
						}
						break;
					}
				case Direction.Up:
					{
						for (int i = 0; i < simulation.Difference; i++)
						{
							SimulationIteration(position => position.Y--);
						}
						break;
					}
				case Direction.Left:
					{
						for (int i = 0; i < simulation.Difference; i++)
						{
							SimulationIteration(position => position.X--);
						}
						break;
					}
				case Direction.Right:
					{
						for (int i = 0; i < simulation.Difference; i++)
						{
							SimulationIteration(position => position.X++);
						}
						break;
					}
			}

			brush.Render(canvas, tmpPosition, simulation.RenderCharacter);
		}

		/// <summary>
		/// Useful when using PathFinder
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="obstacles">All X,Y based coords where obstacles are. X and Y are meant to be <b>ralative</b> to canvas (not absolute to Console)</param>
		/// <exception cref="ArgumentOutOfRangeException">If </exception>
		/// <returns>2 dimensional array of binary values (true means obstacle, false symbolises empty block)</returns>
		public static bool[,] To2DBinaryArray(this Canvas canvas, IEnumerable<Position> obstacles)
		{
			/**
			 * Trim 2 becouse of Width and Height contains walls as well
			 * Implicit value for bool is False -> No need to set it manually
			 */
			bool[,] array = new bool[canvas.Height - 2, canvas.Width - 2];

			foreach (Position obstacle in obstacles)
			{
				array[obstacle.Y, obstacle.X] = true;
			}

			return array;
		}
	}
}