using System;
using System.Linq;
using DrunkenMonk.Data;
using DrunkenMonk.Data.Constants;

namespace DrunkenMonk.ConsoleHelpers
{
	public static class TextCanvasExtensions
	{
		public static void Focus(this TextCanvas canvas)
		{
			throw new NotImplementedException();
		}

		public static void WriteLine(this TextCanvas canvas, string text, int? row = null)
		{
			if (row == null)
				row = canvas.CurrentRow;

			canvas.SetCursorPosition(0, row.Value);

			Console.Write(text);

			canvas.CurrentRow++;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">Is thrown if text would overflow from canvas</exception>
		/// <param name="canvas"></param>
		/// <param name="text"></param>
		/// <param name="row"></param>
		public static void Write(this TextCanvas canvas, string text, int? row = null)
		{
			if (row == null)
				row = canvas.CurrentRow;

			int widthIndex = canvas.ScreenBuffer.ToList()[row.Value].Length - 1;

			// Line-wrapping logic
			if (widthIndex + text.Length > canvas.Width - 2)
			{
				for (int i = canvas.CurrentRow; i < (widthIndex + text.Length) / (canvas.Width - 2); i++)
				{
					int endIndexOfCurrentRow = (i + 1) * canvas.Width;

					if (endIndexOfCurrentRow > text.Length)
						endIndexOfCurrentRow = text.Length - 1;

					string substring = text.Substring(i * (canvas.Width - 3), endIndexOfCurrentRow);

					canvas.SetCursorPosition(widthIndex, canvas.CurrentRow);

					Console.Write(substring);

					canvas.CurrentRow++;
				}

				return;
			}

			canvas.SetCursorPosition(
				widthIndex,
				row.Value);

			Console.Write(text);
		}

		public static void Clear(this TextCanvas canvas, int? rowIndex = null)
		{
			canvas.CurrentRow = 0;

			if (rowIndex == null)
			{
				for (int y = 0; y < canvas.Height - 2; y++)
				{
					canvas.SetCursorPosition(0, y);
					Console.Write(CharMap.Space.Repeat(canvas.Width - 2));
				}
				return;
			}

			// validation
			if (rowIndex.Value > canvas.Height - 2 || rowIndex.Value < 0)
				throw new ArgumentOutOfRangeException(nameof(rowIndex), "Row index cannot overflow canvas's height");

			canvas.SetCursorPosition(0, rowIndex.Value);

			Console.Write(CharMap.Space.Repeat(canvas.Width - 2));
		}
	}
}