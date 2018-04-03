using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DrunkenMonk.Data.Base;

namespace DrunkenMonk.Data
{
	// TODO: Consider adding optional scroll bar
	public class TextCanvas : Canvas
	{
		/// <summary>
		/// Whole Content buffer
		/// </summary>
		private List<string> buffer { get; }

		/// <summary>
		/// Visible content (currentRow - 3 up to (currentRow - 3) + ContentHeigth)
		/// </summary>
		public List<string> ScreenBuffer {
			get
			{
				int start;
				if (_currentRow <= 3)
					start = 0;
				else
					start = _currentRow - 3;

				int end;
				if (start + ContentHeight > buffer.Count - 1 - start)
					end = buffer.Count - 1;
				else
					end = start + ContentHeight;

				return buffer.GetRange(start, end);
			}
		}

		private int _currentRow;

		/// <summary>
		/// Index of row currently selected row to write on
		/// </summary>
		public int CurrentRow
		{
			get => _currentRow;
			set
			{
				if (value > Height || value < 0)
					throw new ArgumentOutOfRangeException(nameof(CurrentRow), "Selected row is out of canvas's height");

				_currentRow = value;
			}
		}

		public TextCanvas()
		{
			 buffer = new List<string>();
		}

		public TextCanvas(List<string> buffer)
		{
			this.buffer = buffer;
		}
	}
}