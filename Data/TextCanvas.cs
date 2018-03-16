using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DrunkenMonk.Data.Base;

namespace DrunkenMonk.Data
{
	public class TextCanvas : Canvas
	{
		/// <summary>
		/// Full buffer
		/// </summary>
		private IEnumerable<string> buffer { get; set; }

		/// <summary>
		/// Visible content
		/// </summary>
		public IEnumerable<string> ScreenBuffer { get; }

		private int _currentRow;

		/// <summary>
		/// Index of row currently selected to write on
		/// </summary>
		public int CurrentRow
		{
			get => _currentRow;
			set
			{
				if (value > Height)
					throw new ArgumentOutOfRangeException(nameof(CurrentRow), "Selected ro");

				_currentRow = value;
			}
		}

		public TextCanvas()
		{
			ScreenBuffer = new List<string>();
		}
	}
}