using UnityEngine;

namespace wtd.spell
{
	/// <summary>
	/// A spell caster should have a unique static <see cref="CasterType>, a <see cref="NextSpell"/> method if it directly shoots spells
	/// </summary>
	public interface ISpellCaster
	{
		/// <summary>
		/// Should be unique and static for each implementation
		/// Naming Format: "SC_camelCase"
		/// </summary>
		/// <returns></returns>
		public string CasterType();

		/// <summary>
		/// Current position of the caster
		/// </summary>
		/// <returns></returns>
		public Vector3 GetPosition();

		/// <summary>
		/// Implement only if it directly shoots spells, like a <see cref="Wand"/>
		/// </summary>
		/// <returns></returns>
		public CasterSpell NextSpell();

		public SpellContainer GetSpellContainer();
	}
}