using System;
using System.Collections.Generic;
using System.Linq;
using DrunkenMonk.Data;
using DrunkenMonk.Data.Constants;
using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.ConsoleHelpers
{
	public static class TextCanvasExtensions
	{
		public static void Focus(
			this TextCanvas canvas,
			ConsoleKeyInfo exitKey,
			Dictionary<ConsoleKeyInfo, Action> keyHandlers)
		{
			/**
			 * TODO: Show Cursor
			 * TODO: Save Input to Buffer
			 * ...
			 */

			throw new NotImplementedException();
		}

		/// <summary>
		/// Overwrites whole line at index of row
		/// </summary>
		/// <param name="canvas"></param>
		/// <param name="text"></param>
		/// <param name="row"></param>
		/// <param name="rightAligned"></param>
		public static void WriteLine(this TextCanvas canvas, string text, int? row = null, bool rightAligned = false)
		{
			row = row ?? canvas.CurrentRow;

			if (row.Value < 0 || row.Value > canvas.ContentHeight)
				throw new ArgumentOutOfRangeException(nameof(row), row.Value, "Given row is out of range");

			#region Line-wrapping Logic

			if (text.Length > canvas.ContentWidth)
			{
				List<string> rows = FoldText(text, canvas.ContentWidth);

				rows.Last().Fill(canvas.ContentWidth, FillOptions.Default | FillOptions.OverwriteBaseString);

				if (rightAligned)
				{
					foreach (var rowString in rows)
					{
						for (int i = rowString.Length - 1, j = 0; i >= 0; i--, j++)
						{
							canvas.SetCursorPosition(canvas.ContentWidth - j, row.Value + rows.IndexOf(rowString));

							Console.Write(rowString[i]);
						}
					}
					return;
				}

				foreach (var rowString in rows)
				{
					canvas.SetCursorPosition(0, row.Value + rows.IndexOf(rowString));

					Console.Write(rowString);
				}
				return;
			}

			#endregion

			if (rightAligned)
			{
				for (int i = text.Length - 1, j = 0; i >= 0 && j < canvas.ContentWidth; i--, j++)
				{
					canvas.SetCursorPosition(canvas.ContentWidth - j, row.Value);

					Console.Write(text[i]);
				}

				return;
			}

			canvas.SetCursorPosition(0, row.Value);

			Console.Write(text);

			canvas.CurrentRow++;
		}

		private static List<string> FoldText(string text, int rowWidth)
		{
			List<string> tmp = new List<string>((int)Math.Ceiling(text.Length / (float)rowWidth));
			for (int i = 0; i < Math.Ceiling(text.Length / (float)rowWidth); i++)
			{
				tmp.Add(text.Substring(i * rowWidth, rowWidth));
			}

			return tmp;
		}

		/// <summary>
		/// Writes text on 1 line, does not handle Line-wrapping
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException">Is thrown if text would overflow from canvas</exception>
		/// <param name="canvas"></param>
		/// <param name="text"></param>
		/// <param name="row">Row of "Visible Content"</param>
		public static void Write(this TextCanvas canvas, string text, int? row = null)
		{
			row = row ?? canvas.CurrentRow;

			int widthIndex = canvas.ScreenBuffer[row.Value].Length;

			// Line-wrapping
			if (widthIndex + text.Length > canvas.ContentWidth)
				throw new ArgumentOutOfRangeException(nameof(text), "Text Overflow");

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
					Console.Write(CharacterMap.Space.Repeat(canvas.ContentWidth));
				}
				return;
			}

			// validation
			if (rowIndex.Value > canvas.Height - 2 || rowIndex.Value < 0)
				throw new ArgumentOutOfRangeException(nameof(rowIndex), "Row index cannot overflow canvas's height");

			canvas.SetCursorPosition(0, rowIndex.Value);

			Console.Write(CharacterMap.Space.Repeat(canvas.ContentWidth));
		}
	}
}