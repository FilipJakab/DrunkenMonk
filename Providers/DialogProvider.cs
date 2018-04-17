using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DrunkenMonk.Data;
using DrunkenMonk.ConsoleHelpers;
using DrunkenMonk.Data.Base;
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
		/// <param name="brush"></param>
		/// <returns></returns>
		public async Task<TReturnType> AskUser<TReturnType>(Menu<TReturnType> menu, PaintBrush brush)
		{
			logger.Trace($"Method {nameof(AskUser)} called");

			Console.Clear();

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

		// todo Implement PromtUser method


		// todo Implement ShowNotification/overload for AskUser method
		// todo Add overload taking string[] param for multiple-row message
		// todo Add param for more display positions (center, top-left etc..)
		/// <summary>
		/// Shows one-row long message
		/// </summary>
		/// <param name="message"></param>
		/// <param name="brush"></param>
		/// <returns></returns>
		public CancellationTokenSource ShowNotification(string message, PaintBrush brush)
		{
			CancellationTokenSource cts = new CancellationTokenSource();
			Task.Run(() =>
			{
				Canvas notificationCanvas = new Canvas
				{
					Width = message.Length + 4,
					Height = 5,
					RenderPosition = RenderPosition.TopLeft
				};
				// render
				brush.RenderCanvas(notificationCanvas);

				for (int i = 0; i < message.Length; i++)
					brush.Render(
						notificationCanvas,
						new Position(notificationCanvas.CenterXPosition - (int)Math.Ceiling(message.Length / 2.0) + i, 1),
						message[i]);

				// wait for cancelation
				WaitHandle.WaitAll(new[] { cts.Token.WaitHandle });

				// derender
				brush.DerenderCanvas(notificationCanvas);
			}, cts.Token);

			return cts;
		}

		public CancellationToken ShowNotification(Func<string> animatedText, PaintBrush brush, TimeSpan interval)
		{
			throw new NotImplementedException();
		}
	}
}