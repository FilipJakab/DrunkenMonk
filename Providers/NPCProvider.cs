using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DrunkenMonk.Data;
using DrunkenMonk.Data.Base;
using DrunkenMonk.Data.Enums;
using NLog;

namespace DrunkenMonk.Providers
{
	public class NPCProvider
	{
		private readonly Logger logger;

		public NPCProvider()
		{
			logger = LogManager.GetCurrentClassLogger();
		}

		/// <summary>
		/// Generates Enemies with Relavtive postions to canvas
		/// </summary>
		/// <param name="enemies"></param>
		/// <param name="canvas"></param>
		/// <param name="player"></param>
		/// <param name="amount"></param>
		public List<Enemy> GenerateEnemies(Canvas canvas, Player player, int amount)
		{
			logger.Trace($"{nameof(GenerateEnemies)} method called");

			Random random = new Random(DateTime.Now.Millisecond);

			List<Enemy> enemies = new List<Enemy>((int) amount);

			for (int i = 0; i < amount; i++)
			{
				// Positions are relative
				int x = random.Next(0, canvas.Width - 2);
				int y = random.Next(0, canvas.Height - 2);

				if (x == player.Position.X && y == player.Position.Y)
				{
					i--;
					continue;
				}

				enemies.Add(new Enemy
				{
					Position = new Position
					{
						X = x,
						Y = y
					}
				});
			}

			return enemies;
		}
	}
}