

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
			string tmp = "";
			for (int i = 0; i < n; i++)
				tmp += x;
			return tmp;
		}
	}
}