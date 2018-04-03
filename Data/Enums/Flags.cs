using System;

namespace DrunkenMonk.Data.Enums
{
	[Flags]
	public enum FillOptions
	{
		Default = 0b000,							// 000
		Prepend = 0b001,							// 001
		Truncate = 0b010,							// 010
		OverwriteBaseString = 0b100		// 100
	}
}