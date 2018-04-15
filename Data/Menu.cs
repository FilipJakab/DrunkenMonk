using System;
using System.Collections.Generic;
using System.Linq;
using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.Data
{
	public class Menu<TReturnType>
	{
		// TODO: Add cancelation option

		public string Question { get; set; }

		public Dictionary<TReturnType, string> Choices { get; set; }

		private KeyValuePair<TReturnType, string>? _selectedChoice;

		public KeyValuePair<TReturnType, string> SelectedChoice
		{
			get => _selectedChoice ?? Choices.First();
			set => _selectedChoice = value;
		}

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

		/// <summary>
		/// Y axis level where the whole menu starts (Question Row included)
		/// </summary>
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

		/// <summary>
		/// Margin around menu
		/// </summary>
		public int Margin { get; set; } = 1;

		public int CenterXPosition => StartX + (MenuWidth / 2);

		/// <summary>
		/// Returns the widht of the Menu dialog (selector characters included)
		/// </summary>
		public int MenuWidth
		{
			get
			{
				int maximalOption = Choices.Max(o => o.Value.Length);

				return (maximalOption > Question.Length
					? maximalOption
					: Question.Length) + 4; // +4 becouse of selectors
			}
		}

		public string LeftSelector { get; set; } = "[";

		public string RightSelector { get; set; } = "]";

		public int SelectorDistance { get; set; } = 1;

		/// <summary>
		/// Returns Total rows for current dialog (The Question/Title included)
		/// </summary>
		public int Rows => Choices.Count + (!string.IsNullOrEmpty(Question) ? 0 : 1);
	}

	public class Menu
	{
		public enum OptionChangeDirection
		{
			Up,
			Down
		}
	}
}
