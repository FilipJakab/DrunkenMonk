using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.ConsoleHelpers
{
	public static class StringExtensions
	{
		public static string Repeat(this string x, int n)
		{
			string tmp = "";
			for (int i = 0; i < n; i++)
				tmp += x;
			return tmp;
		}

		public static string Repeat(this char x, int n)
		{
			return x.ToString().Repeat(n);
		}

		/// <summary>
		/// Fills string to given length
		/// </summary>
		/// <param name="x"></param>
		/// <param name="length"></param>
		/// <param name="filler"></param>
		///// <param name="prepend"></param>
		///// <param name="truncate">If used with prepend option: method will return string of given length from end of original string</param>
		///// <param name="overwriteBaseString"></param>
		/// <param name="options">If used truncate with prepend option, the returned string will be token from the end of original string. Defualt is (no prepend, no truncate, no overwrite)</param>
		/// <returns></returns>
		public static string Fill(
			this string x,
			int length,
			FillOptions options,
			char filler = ' ')
		{
			#region Solution 1

			//if (x.Length > length)
			//{
			//	return truncate 
			//		? x.Substring(0, length)
			//		: x;
			//}

			//return prepend
			//	? filler.Repeat(length - x.Length) + x
			//	: x + filler.Repeat(length - x.Length);

			#endregion

			#region Solution 2

			//if (x.Length > length)
			//{
			//	if (!truncate) return x;
			//	x = x.Substring(0, length);
			//	return x;
			//}

			//if (prepend)
			//{
			//	x = filler.Repeat(length - x.Length) + x;
			//	return x;
			//}

			//x = x + filler.Repeat(length - x.Length);
			//return x;

			#endregion

			#region Solution 3 with Flags

			if (x.Length > length)
			{
				// No truncate option
				if ((options & FillOptions.Truncate) == 0)
					return x;

				// Truncate with Prepend
				if ((options & (FillOptions.Truncate | FillOptions.Prepend)) != 0)
				{
					// No overwrite
					if ((options & FillOptions.OverwriteBaseString) == 0)
						return x.Substring(x.Length - length - 1);

					x = x.Substring(x.Length - length - 1);
					return x;
				}
			}

			// Prepend option
			if ((options & FillOptions.Prepend) != 0)
			{
				if ((options & FillOptions.OverwriteBaseString) == 0)
					return filler.Repeat(length - x.Length) + x;

				x = filler.Repeat(length - x.Length) + x;
				return x;
			}

			if ((options & FillOptions.OverwriteBaseString) == 0)
				return x + filler.Repeat(length - x.Length);

			x = x + filler.Repeat(length - x.Length);
			return x;

			#endregion
		}
	}
}