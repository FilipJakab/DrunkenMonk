using System;
using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.ConsoleHelpers
{
	public static class DirectionExtensions
	{
		/// <summary>
		/// Returns the opposite direction
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Direction Reverse(this Direction direction)
		{
			switch (direction)
			{
				case Direction.Down:
					{
						return Direction.Up;
					}
				case Direction.Up:
					{
						return Direction.Down;
					}
				case Direction.Left:
					{
						return Direction.Right;
					}
				case Direction.Right:
					{
						return Direction.Left;
					}
				default:
				{
					throw new ArgumentOutOfRangeException(nameof(direction));
				}
			}


		}
	}
}