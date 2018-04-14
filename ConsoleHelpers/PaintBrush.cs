using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DrunkenMonk.Data;
using DrunkenMonk.Data.Base;
using DrunkenMonk.Data.Constants;
using NLog;

namespace DrunkenMonk.ConsoleHelpers
{
	public class PaintBrush
	{
		private readonly Logger logger;

		private readonly bool leaveTrail;

		public PaintBrush(bool leaveTrail = false)
		{
			logger = LogManager.GetCurrentClassLogger();

			this.leaveTrail = leaveTrail;
		}

		/// <summary>
		/// Draws basic canvas surrounded by walls
		/// Lets a space for socre board on right side of Console screen
		/// </summary>
		public void RenderCanvas(Canvas canvas)
		{
			logger.Trace($"Rendering canvas {canvas.Title}");

			DrawRectangle(canvas.StartX, canvas.StartY, canvas.Width, canvas.Height);

			canvas.SetCursorPosition(2, -1, false);
			Console.Write(CharMap.Space + canvas.Title + CharMap.Space);
		}

		public void ShowPath(
			Canvas canvas,
			List<Position> path,
			int visibleLength = 10,
			ConsoleColor? foregroundColor = null,
			ConsoleColor? backgroundColor = null,
			bool animated = false,
			bool underlineTrail = false,
			char? underlineChar = null,
			int animationDelayMillis = 250,
			int visibleFor = 1000,
			char renderChar = CharMap.FullBlock)
		{
			// No need of derendering => will derendered as visiblePath is rendering/derendering
			if (underlineTrail)
				Render(canvas, path, underlineChar ?? CharMap.LightTrail);

			for (int i = 0; i < path.Count; i++)
			{
				Render(canvas, path[i], renderChar,
					foregroundColor ?? Console.ForegroundColor,
					foregroundColor ?? Console.ForegroundColor);

				if (animated)
					Thread.Sleep(animationDelayMillis);

				if (i >= visibleLength)
					Derender(canvas, path[i - visibleLength], underlineTrail, underlineChar);
			}

			for (int i = path.Count - 1 - visibleLength; i < path.Count; i++)
			{
				if (animated)
					Thread.Sleep(animationDelayMillis);

				Derender(canvas, path[i], underlineTrail, underlineChar);
			}

			Thread.Sleep(visibleFor);

			// Derender remaining positions
			foreach (var position in path)
				Derender(canvas, position);
		}

		/// <param name="menu"></param>
		/// <param name="optionFormat">Format of question to be displayed</param>
		public void RenderMenu<T>(Menu<T> menu, string optionFormat = "{0}")
		{
			logger.Trace("Method for rendering Menu called");

			int startX = menu.StartX,
				startY = menu.StartY;

			// Render title
			if (!string.IsNullOrEmpty(menu.Question))
			{
				Console.SetCursorPosition(
					menu.CenterXPosition - (menu.Question.Length / 2),
					startY++
				);
				Console.Write(menu.Question);
			}

			// RenderMenu Options
			foreach (KeyValuePair<T, string> option in menu.Options)
			{
				bool isSelected = Equals((menu.SelectedOption ?? menu.Options.First()), option);

				if (isSelected)
				{
					Console.SetCursorPosition(menu.CenterXPosition - (option.Value.Length / 2) - 3, startY);
					Console.Write('[');
				}

				Console.SetCursorPosition(menu.CenterXPosition - (option.Value.Length / 2), startY++);
				Console.Write(optionFormat, option.Value);

				if (!isSelected) continue;

				Console.Write("  ]");
			}

			menu.SelectedOption = menu.Options.First();
		}

		public void SelectOption<T>(Menu<T> menu, Menu<T>.OptionChangeDirection newDirection)
		{
			logger.Trace("Method for selecting another option of Menu called");

			int startY = menu.StartY + 1;

			#region Trim selected option

			int lastIndex = menu.Options.ToList().IndexOf(menu.SelectedOption ?? menu.Options.ToList().First());

			// Validation
			if (lastIndex == 0 && newDirection == Menu<T>.OptionChangeDirection.Up
					|| lastIndex == menu.Options.Count - 1 && newDirection == Menu<T>.OptionChangeDirection.Down)
				return;

			Console.SetCursorPosition(
				menu.CenterXPosition - ((menu.SelectedOption?.Value.Length ?? menu.Options.First().Value.Length) / 2) - 3,
				startY + lastIndex);
			Console.Write(CharMap.Space);

			Console.SetCursorPosition(
				menu.CenterXPosition + ((menu.SelectedOption?.Value.Length ?? menu.Options.First().Value.Length) / 2) + 3,
				startY + lastIndex);
			Console.Write(CharMap.Space);

			#endregion

			#region Select new option

			int newIndex;

			switch (newDirection)
			{
				case Menu<T>.OptionChangeDirection.Up:
					{
						newIndex = lastIndex - 1;

						break;
					}
				case Menu<T>.OptionChangeDirection.Down:
					{
						newIndex = lastIndex + 1;

						break;
					}
				default:
					{
						newIndex = 0;
						break;
					}
			}

			menu.SelectedOption = menu.Options.ToArray()[newIndex];

			Console.SetCursorPosition(
				menu.CenterXPosition - (menu.SelectedOption.Value.Value.Length / 2) - 3,
				startY + newIndex);
			Console.Write('[');

			Console.SetCursorPosition(
				menu.CenterXPosition + (menu.SelectedOption.Value.Value.Length / 2) + 3,
				startY + newIndex);
			Console.Write(']');

			#endregion
		}

		/// <summary>
		/// Removes character at the specified position relatively to canvas
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="position"></param>
		/// <param name="trailChar"></param>
		public void Derender(Canvas canvas, Position position, char? trailChar = null)
		{
			Derender(canvas, position, leaveTrail, trailChar);
		}

		private void Derender(Canvas canvas, Position position, bool leaveTrail, char? trailChar = null)
		{
			logger.Trace("Derendering position");

			logger.Debug($"Derendering position {{{position.X}, {position.Y}}} in canvas {canvas.Title}");

			canvas.SetCursorPosition(position.X, position.Y);

			char renderChar = leaveTrail ? (trailChar ?? CharMap.LightTrail) : CharMap.Space;

			Console.Write(renderChar);
		}

		public void Derender(Canvas canvas, IEnumerable<Position> obstacles)
		{
			logger.Trace("Derendering positions");

			Position[] positions = obstacles as Position[] ?? obstacles.ToArray();

			logger.Debug($"Derendering {positions.Length} positions in canvas {canvas.Title}");

			foreach (Position position in positions)
			{
				canvas.SetCursorPosition(position.X, position.Y);
				Console.Write(CharMap.Space);
			}
		}

		public void Derender(Canvas canvas, bool[,] positions, char? trailChar = null)
		{
			for (int y = 0; y < positions.GetLength(0); y++)
			{
				for (int x = 0; x < positions.GetLength(1); x++)
				{
					Derender(canvas, new Position(x, y), trailChar);
				}
			}
		}

		public void EraseCanvas(Canvas canvas)
		{
			for (int y = 0; y < canvas.ContentHeight; y++)
			{
				for (int x = 0; x < canvas.ContentWidth; x++)
				{
					canvas.SetCursorPosition(x, y);
					Console.Write(CharMap.Space);
				}
			}
		}

		public void Render(
			Canvas canvas,
			Position position,
			char character,
			ConsoleColor? foregroundColor = null,
			ConsoleColor? backgroundColor = null)
		{
			ConsoleColor baseForegroundColor = Console.ForegroundColor;
			ConsoleColor baseBackgroundColor = Console.BackgroundColor;

			Console.BackgroundColor = backgroundColor ?? baseBackgroundColor;
			Console.ForegroundColor = foregroundColor ?? baseForegroundColor;

			logger.Trace("Rendering position");

			logger.Debug($"Rendering character at {{{position.X}, {position.Y}}} in canvas: {canvas.Title}");

			canvas.SetCursorPosition(position.X, position.Y);

			Console.Write(character);

			Console.ForegroundColor = baseForegroundColor;
			Console.BackgroundColor = baseBackgroundColor;
		}

		public void Render(
			Canvas canvas,
			List<Position> obstacles,
			char character,
			ConsoleColor? foregroundColor = null,
			ConsoleColor? backgroundColor = null)
		{
			logger.Trace("Rendering popsitions");

			logger.Debug($"Rendering {obstacles.Count} characters in canvas {canvas.Title}");

			foreach (Position position in obstacles)
				Render(canvas, position, character,
					foregroundColor,
					backgroundColor);
		}

		public void Render(Canvas canvas, IEnumerable<Data.PathFinder.Position> obstacles, TimeSpan delay)
		{
			foreach (Data.PathFinder.Position obstacle in obstacles)
			{
				Thread.Sleep(delay);
				Render(canvas, new Position(obstacle.X, obstacle.Y), obstacle.DistanceFromStart.ToString().Last());
			}
		}

		public void Render(
			Canvas canvas,
			bool[,] positions,
			char trueChar = CharMap.DarkTrail,
			char falseChar = CharMap.Space)
		{
			for (int y = 0; y < positions.GetLength(0); y++)
			{
				for (int x = 0; x < positions.GetLength(1); x++)
				{
					Render(canvas, new Position(x, y), positions[y, x]
					? trueChar
					: falseChar);
				}
			}
		}

		public void DrawRectangle(int startX, int startY, int width, int height)
		{
			logger.Trace("Drawing rectangle");

			logger.Debug($"Drawing rectagle of {width} width and {height} height at {{{startX},{startY}}}");

			// substract walls (2)
			// LE becouse width & height contains start...
			for (int y = startY; y < startY + height; y++)
			{
				for (int x = startX; x < startX + width; x++)
				{
					/**
					 * Console.SetCursorPosition(x, y);
					 * is separated into each branch because otherwise it would be being set each loop iteration...
					 * Thats why is this solution better - faster (I think) :)
					 */
					if (y == startY && x == startX)
					{
						Console.SetCursorPosition(x, y);
						Console.Write(CharMap.TopLeftCornerWall);
					}
					else if (y == startY && x == startX + width - 1)
					{
						Console.SetCursorPosition(x, y);
						Console.Write(CharMap.TopRightCornerWall);
					}
					else if (y == startY + height - 1 && x == startX)
					{
						Console.SetCursorPosition(x, y);
						Console.Write(CharMap.DownLeftCornerWall);
					}
					else if (y == startY + height - 1 && x == startX + width - 1)
					{
						Console.SetCursorPosition(x, y);
						Console.Write(CharMap.DownRightCornerWall);
					}
					else if (y == startY || y == startY + height - 1)
					{
						Console.SetCursorPosition(x, y);
						Console.Write(CharMap.HorizontalWall);
					}
					else if (x == startX || x == startX + width - 1)
					{
						Console.SetCursorPosition(x, y);
						Console.Write(CharMap.VerticalWall);
					}
				}
			}
		}
	}
}