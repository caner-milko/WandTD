using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	public class CasterSpell : IComparable<CasterSpell>
	{
		public SpellData spellData { get; private set; }
		public bool isActive { get; private set; }
		public ISpellCaster owner { get; private set; }

		int slot;

		public CasterSpell(SpellData spellData, ISpellCaster owner, int slot)
		{
			this.spellData = spellData;
			this.owner = owner;
			this.slot = slot;
			this.isActive = spellData is ActiveSpellData;
		}

		public int CompareTo(CasterSpell other)
		{
			return this.slot.CompareTo(other.slot);
		}
	}
}
