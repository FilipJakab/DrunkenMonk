using System;
using System.Configuration;

namespace DrunkenMonk.Extensions
{
	public static class Configurations
	{
		public static string GetConsoleTitle => ConfigurationManager.AppSettings["ConsoleTitle"];

		/// <exception cref="FormatException">Config File doesnt have to be valid consider try-catch</exception>
		public static int GetConsoleWidth => int.Parse(ConfigurationManager.AppSettings["ConsoleWidth"]);

		/// <exception cref="FormatException">Config File doesnt have to be valid consider try-catch</exception>
		public static int GetConsoleHeight => int.Parse(ConfigurationManager.AppSettings["ConsoleHeight"]);

		/// <exception cref="FormatException">Config File doesnt have to be valid consider try-catch</exception>
		public static int GetComponentMargin => int.Parse(ConfigurationManager.AppSettings["ComponentMargin"]);

		/// <exception cref="FormatException">Config File doesnt have to be valid consider try-catch</exception>
		public static int GetScoreBoardWidth => int.Parse(ConfigurationManager.AppSettings["ScoreBoardWidth"]);

		/// <exception cref="FormatException">Config File doesnt have to be valid consider try-catch</exception>
		public static int GetSquareWidth => GetConsoleWidth - (GetComponentMargin * 4) - GetScoreBoardWidth;

		/// <exception cref="FormatException">Config File doesnt have to be valid consider try-catch</exception>
		public static int GetSquareHeight => GetConsoleHeight - (GetComponentMargin * 2);

		public static int GetMainDelay => int.Parse(ConfigurationManager.AppSettings["MainDelay"]);
	}
}