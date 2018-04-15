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
			logger.Trace($"Method {nameof(AskUser)} called");

			Console.Clear();

			PaintBrush brush = new PaintBrush();

			brush.RenderMenu(menu);

			ConsoleKey key;

			do
			{
				key = await Task.Run(() => Console.ReadKey(true).Key);

				switch (key)
				{
					case ConsoleKey.UpArrow:
					{
						int newIndex = menu.Choices.ToList().IndexOf(menu.SelectedChoice) - 1;
						// overflow validation
						// todo implement "rotating option for Menu model" (if selected q is 0 and direction is UP select last q and vise versa)
						if (newIndex < 0)
							continue;

						brush.DeselectChoice(menu);

						menu.SelectedChoice = menu.Choices.ToList()[newIndex];

						brush.SelectChoice(menu);
						break;
					}
					case ConsoleKey.DownArrow:
					{
						int newIndex = menu.Choices.ToList().IndexOf(menu.SelectedChoice) + 1;

						// todo implement "rotating option for Menu model" (if selected q is 0 and direction is UP select last q and vise versa)
						if (newIndex >= menu.Choices.Count)
							continue;

						brush.DeselectChoice(menu);

						menu.SelectedChoice = menu.Choices.ToList()[newIndex];

						brush.SelectChoice(menu);
						break;
					}
				}
			} while (key != ConsoleKey.Enter);

			Console.Clear();

			logger.Trace($"Method {nameof(AskUser)} ended");

			return menu.SelectedChoice.Key;
		}

		// todo implement PromtUser method
	}
}