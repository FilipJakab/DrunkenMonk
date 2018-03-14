using System.Collections.Generic;
using System.Linq;

namespace DrunkenMonk.Data
{
	public class Menu<TReturnType>
	{
		public string Question { get; set; }

		public Dictionary<TReturnType, string> Options { get; set; }

		public int MenuWidth => Options.Max(q => q.Value.Length);

		public int Rows => Options.Count;

		public KeyValuePair<TReturnType, string> SelectedOption { get; set; }

		public enum OptionChangeDirection
		{
			Up,
			Down
		}
	}
}
