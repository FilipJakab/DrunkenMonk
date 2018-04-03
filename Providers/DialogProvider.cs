using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DrunkenMonk.Data;
using DrunkenMonk.ConsoleHelpers;
using DrunkenMonk.Data.Constants;
using DrunkenMonk.Data.Enums;
using NLog;

namespace DrunkenMonk.Providers
{
	public class DialogProvider
	{
		private readonly Logger logger;

		public DialogProvider()
		{
			logger = LogManager.GetCurrentClassLogger();
		}

		/// <summary>
		/// Clears Cosole Screen and draws menu of provided options
		/// </summary>
		/// <typeparam name="TReturnType"></typeparam>
		/// <param name="menu"></param>
		/// <returns></returns>
		public async Task<TReturnType> AskUser<TReturnType>(Menu<TReturnType> menu)
		{
			logger.Trace($"{nameof(AskUser)} method called");

			Console.Clear();

			PaintBrush brush = new PaintBrush();

			brush.RenderMenu(menu);

			ConsoleKey key;

			do
			{
				key = await Task.Run(() => Console.ReadKey().Key);

				switch (key)
				{
					case ConsoleKey.UpArrow:
						{
							// overflow validation
							if (menu.Options.ToList().IndexOf(menu.SelectedOption ?? menu.Options.First()) == 0)
								continue;

							brush.SelectOption(menu, Menu<TReturnType>.OptionChangeDirection.Up);
							break;
						}
					case ConsoleKey.DownArrow:
						{
							if (menu.Options.ToList().IndexOf(menu.SelectedOption ?? menu.Options.First()) == menu.Options.Count - 1)
								continue;

							brush.SelectOption(menu, Menu<TReturnType>.OptionChangeDirection.Down);
							break;
						}
				}
			} while (key != ConsoleKey.Enter);

			Console.Clear();


			return menu.SelectedOption != null
				? menu.SelectedOption.Value.Key
				: menu.Options.First().Key;
		}

		///// <summary>
		///// Selects Next question.
		///// There must be selected question in Menu (via Menu.RenderMenu())
		///// </summary>
		///// <typeparam name="T"></typeparam>
		///// <param name="menu"></param>
		///// <param name="direction"></param>
		//public void SelectQuestion<T>(Menu<T> menu, Menu<T>.OptionChangeDirection direction)
		//{
		//	logger.Trace($"{nameof(SelectQuestion)} method called");

		//	int startY = (Console.WindowHeight - menu.Rows) / 2,
		//		startX = (Console.WindowWidth - menu.MenuWidth) / 2;

		//	#region Trim last selected O. from UI

		//	KeyValuePair<T, string> lastSelectedQuestion = menu.SelectedOption ?? menu.Options.First();

		//	// Get Index of selected row
		//	int lastIndex = menu.Options.ToList().IndexOf(lastSelectedQuestion);

		//	// Trim prefix-bracket
		//	Console.SetCursorPosition(startX - 2, startY + lastIndex);
		//	Console.Write(CharMap.Space);

		//	// Trim postfix-bracket
		//	Console.SetCursorPosition(
		//		startX + (menu.SelectedOption?.Value.Length ?? menu.Options.First().Value.Length) - 1 + 2, startY + lastIndex);
		//	Console.Write(CharMap.Space);

		//	#endregion

		//	#region Select O. in UI

		//	int newIndex = menu.Options
		//									 .ToList().IndexOf(lastSelectedQuestion) + (direction == Menu<T>.OptionChangeDirection.Down ? 1 : -1);

		//	if (newIndex < 0 || newIndex >= menu.Options.Count)
		//		return;

		//	menu.SelectedOption = menu.Options.ToList()[newIndex];

		//	Console.SetCursorPosition(startX - 2, startY + newIndex);
		//	Console.Write('[');

		//	Console.SetCursorPosition(startX + menu.SelectedOption.Value.Length - 1 + 2, startY + newIndex);
		//	Console.Write(']');

		//	#endregion
		//}
	}
}