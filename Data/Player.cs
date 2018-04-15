using DrunkenMonk.Data.Base;
using DrunkenMonk.Data.Constants;
using DrunkenMonk.Data.Enums;

namespace DrunkenMonk.Data
{
	public class Player : Entity
	{
		public const char BodyCharacter = CharacterMap.Player;

		public DifficultyLevel DifficultyLevel { get; set; }

		public Direction Direction { get; set; }
	}
}