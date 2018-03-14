using System;
using System.Configuration;

namespace DrunkenMonk
{
	public class Configurations
	{
		public string GetConsoleTitle => ConfigurationManager.AppSettings["ConsoleTitle"];

		/// <exception cref="FormatException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public int GetConsoleWidth => int.Parse(ConfigurationManager.AppSettings["ConsoleWidth"]);

		/// <exception cref="FormatException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public int GetConsoleHeight => int.Parse(ConfigurationManager.AppSettings["ConsoleHeight"]);

		/// <exception cref="FormatException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public int GetComponentMargin => int.Parse(ConfigurationManager.AppSettings["ComponentMargin"]);

		/// <exception cref="FormatException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public int GetSquareWidth => int.Parse(ConfigurationManager.AppSettings["SquareWidth"]);

		/// <exception cref="FormatException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public int GetSquareHeight => GetConsoleHeight - (GetComponentMargin * 2);

		/// <exception cref="FormatException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public int GetScoreBoardWidth => GetConsoleWidth - (GetComponentMargin * 4) - GetSquareWidth;

		/// <exception cref="FormatException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public int GetScoreBoardHeight => GetConsoleHeight - (GetComponentMargin * 2);

		/// <exception cref="FormatException"></exception>
		/// <exception cref="ArgumentNullException"></exception>
		public int GetMainDelay => int.Parse(ConfigurationManager.AppSettings["MainDelay"]);
	}
}