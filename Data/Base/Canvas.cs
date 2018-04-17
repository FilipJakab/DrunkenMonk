using System;
using System.Globalization;
using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.Data.Base
{
	public class Canvas
	{
		private string _title;

		private int? _startX = null;

		private int? _startY = null;

		public int StartX
		{
			get
			{
				if (_startX != null)
					return _startX.Value;

				switch (RenderPosition)
				{
					case RenderPosition.TopLeft:
						return Margin;
					case RenderPosition.TopRight:
						return Console.WindowWidth - Width + Margin;
					case RenderPosition.Center:
						return (Console.WindowWidth - Width) / 2;
					case RenderPosition.BottomLeft:
						return Margin;
					case RenderPosition.BottomRight:
						return Console.WindowWidth - Width - Margin;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set => _startX = value;
		}

		public int StartY
		{
			get
			{
				if (_startY != null)
					return _startY.Value;

				switch (RenderPosition)
				{
					case RenderPosition.TopLeft:
						return Margin;
					case RenderPosition.TopRight:
						return Margin;
					case RenderPosition.Center:
						return (Console.WindowHeight - Height) / 2;
					case RenderPosition.BottomLeft:
						return Console.WindowHeight - Height - Margin;
					case RenderPosition.BottomRight:
						return Console.WindowHeight - Height - Margin;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			set => _startY = value;
		}

		public RenderPosition RenderPosition { get; set; } = RenderPosition.Center;

		public int Margin { get; set; } = 1;

		public int Width { get; set; }

		public int ContentWidth => Width - 2;

		public int Height { get; set; }

		public int ContentHeight => Height - 2;

		public int CenterXPosition => ContentWidth / 2;

		//public char BorderChar { get; set; }

		public string Title
		{
			get => _title;
			set
			{
				if (value.Length > Width)
					throw new ArgumentOutOfRangeException($"{nameof(Title)}: {value} is longer than width of Canvas");

				_title = value;
			}
		}
	}
}