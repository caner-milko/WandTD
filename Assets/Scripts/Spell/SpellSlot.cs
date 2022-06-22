using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace wtd.spell
{
	public class SpellSlot
	{
		public CasterSpell Spell = null;

		public readonly SpellContainer belongsTo;

		public bool IsEmpty => Spell == null;
		public readonly int slot;
		public SpellSlot(CasterSpell spell, int slot, SpellContainer inside)
		{
			Spell = spell;
			this.slot = slot;
			this.belongsTo = inside;
		}
	}
}
