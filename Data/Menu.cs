﻿using System;
using System.Collections.Generic;
using System.Linq;
using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.Data
{
	public class Menu<TReturnType>
	{
		// TODO: Add cancelation option

		public string Question { get; set; }

		public Dictionary<TReturnType, string> Options { get; set; }

		public RenderMenuPosition Position { get; set; } = RenderMenuPosition.Center;

		public int StartX
		{
			get
			{
				switch (Position)
				{
					case RenderMenuPosition.TopLeft:
						return Margin;
					case RenderMenuPosition.TopRight:
						return Console.WindowWidth - MenuWidth + Margin;
					case RenderMenuPosition.Center:
						return (Console.WindowWidth - MenuWidth) / 2;
					case RenderMenuPosition.BottomLeft:
						return Margin;
					case RenderMenuPosition.BottomRight:
						return Console.WindowWidth - MenuWidth - Margin;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public int StartY
		{
			get
			{
				switch (Position)
				{
					case RenderMenuPosition.TopLeft:
						return Margin;
					case RenderMenuPosition.TopRight:
						return Margin;
					case RenderMenuPosition.Center:
						return (Console.WindowHeight - Rows) / 2;
					case RenderMenuPosition.BottomLeft:
						return Console.WindowHeight - Rows - Margin;
					case RenderMenuPosition.BottomRight:
						return Console.WindowHeight - Rows - Margin;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		public int Margin { get; set; } = 1;

		public int CenterXPosition => StartX + (MenuWidth / 2);

		/// <summary>
		/// Returns the widht of the Menu dialog (selector characters included)
		/// </summary>
		public int MenuWidth
		{
			get
			{
				int maximalOption = Options.Max(q => q.Value.Length);

				return (maximalOption > Question.Length
					? maximalOption
					: Question.Length) + 6;
			}
		}

		/// <summary>
		/// Returns Total rows for current dialog (The Question included)
		/// </summary>
		public int Rows => Options.Count + 1;

		public KeyValuePair<TReturnType, string>? SelectedOption { get; set; }

		public enum OptionChangeDirection
		{
			Up,
			Down
		}
	}
}
