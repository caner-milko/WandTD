using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	public class SpellSlot
	{
		public CasterSpell Spell = null;

		public bool IsEmpty => Spell == null;

		public SpellSlot(CasterSpell spell)
		{
			Spell = spell;
		}
	}
}
