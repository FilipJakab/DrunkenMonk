using System;
using System.Globalization;

namespace DrunkenMonk.Data.Base
{
	public class Canvas
	{
		private string _title;

		public int StartX { get; set; }

		public int StartY { get; set; }

		public int Width { get; set; }

		public int ContentWidth => Width - 2;

		public int Height { get; set; }

		public int ContentHeight => Height - 2;

		public char BorderChar { get; set; }

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