using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using DrunkenMonk.Data;
using DrunkenMonk.Data.Base;
using DrunkenMonk.Data.Constants;
using DrunkenMonk.Data.Enums;
using DrunkenMonk.Data.Exceptions;

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
			if (validateInput)
			{
				// Substract walls
				if (left >= canvas.ContentWidth || left < 0)
					return;// throw new ArgumentOutOfRangeException(nameof(left), $"Parameter is out of Map; width: {canvas.Width}, left: {left}");
				if (top >= canvas.ContentHeight || top < 0)
					return; // throw new ArgumentOutOfRangeException(nameof(top), $"Parameter is out of Map; content height: {canvas.ContentHeight}, top: {top}");
			}

			// + 1 because top-left border is at StartX and StartY
			Console.SetCursorPosition(
				left + canvas.StartX + 1,
				top + canvas.StartY + 1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="simulation"></param>
		/// <param name="validate">Return true if position is valid</param>
		/// <param name="brush"></param>
		/// <param name="isTrip"></param>
		/// <param name="color"></param>
		/// <returns></returns>
		public static SimulationResult ExecuteSimulation(
			this Canvas canvas,
			Simulation simulation,
			Func<Position, bool> validate,
			PaintBrush brush,
			bool isTrip = false,
			ConsoleColor? color = null)
		{
			// BassePosition is obstacle
			//brush.Derender(canvas, simulation.BasePosition);
			Position currentPosition = Position.Copy(simulation.BasePosition);

			SimulationResult result = new SimulationResult();

			ConsoleColor baseForegroundColor = Console.ForegroundColor;
			ConsoleColor baseBackgroundColor = Console.BackgroundColor;

			void SimulationIteration(Action<Position> modification)
			{
				if (isTrip)
					brush.Derender(canvas, currentPosition, CharacterMap.MediumTrail);

				modification(currentPosition);

				if (!validate(currentPosition))
				{
					result.HasSuccessfulyFinished = false;
					result.Obstacle = Position.Copy(currentPosition);

					return;
				}

				// TODO: Move to config/constnats
				Console.ForegroundColor = ConsoleColor.Cyan;

				brush.Render(canvas, currentPosition, simulation.RenderCharacter);

				Console.ResetColor();

				// TODO: Move delayTime to constants / Appconfig
				Thread.Sleep(350);

				brush.Derender(canvas, currentPosition);

				result.LastSafePosition = Position.Copy(currentPosition);
				result.Obstacle = null;
				result.HasSuccessfulyFinished = true;
			}

			switch (simulation.Direction)
			{
				case Direction.Down:
				{
					for (int i = 0; i < simulation.Difference; i++)
					{
						SimulationIteration(position => position.Y++);

						if (!result.HasSuccessfulyFinished)
							goto End;
					}
					break;
				}
				case Direction.Up:
				{
					for (int i = 0; i < simulation.Difference; i++)
					{
						SimulationIteration(position => position.Y--);

						if (!result.HasSuccessfulyFinished)
							goto End;
					}
					break;
				}
				case Direction.Left:
				{
					for (int i = 0; i < simulation.Difference; i++)
					{
						SimulationIteration(position => position.X--);

						if (!result.HasSuccessfulyFinished)
							goto End;
					}
					break;
				}
				case Direction.Right:
				{
					for (int i = 0; i < simulation.Difference; i++)
					{
						SimulationIteration(position => position.X++);

						if (!result.HasSuccessfulyFinished)
							goto End;
					}
					break;
				}
			}

			brush.Render(canvas, currentPosition, simulation.RenderCharacter);

			result.HasSuccessfulyFinished = true;

			End:

			Console.ForegroundColor = baseForegroundColor;
			Console.BackgroundColor = baseBackgroundColor;

			return result;
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
			bool[,] array = new bool[canvas.ContentHeight, canvas.ContentWidth];

			foreach (Position obstacle in obstacles)
			{
				array[obstacle.Y, obstacle.X] = true;
			}

			return array;
		}

		/// <summary>
		/// A Generic method allowing to convert Array of positions(even classes deriving from position) to 2D
		/// </summary>
		/// <typeparam name="TX"></typeparam>
		/// <typeparam name="TY"></typeparam>
		/// <param name="canvas"></param>
		/// <param name="positions"></param>
		/// <param name="transformation"></param>
		/// <returns></returns>
		public static TX[,] To2DArray<TX, TY>(this Canvas canvas, IEnumerable<TY> positions, Func<TY, TX> transformation)
			where TY : class
		{
			// Method with no current usage, just playing with some generics...

			// Check if TY inherits from Position class
			Type baseType = typeof(TY).BaseType;
			if (baseType == null || baseType != typeof(Position))
				throw new InvalidTypeException();

			TX[,] toReturn = new TX[canvas.ContentHeight, canvas.ContentWidth];

			foreach (TY position in positions)
			{
				Position converted = (Position)(object)position;
				toReturn[converted.Y, converted.X] = transformation(position);
			}

			return toReturn;
		}
	}
}