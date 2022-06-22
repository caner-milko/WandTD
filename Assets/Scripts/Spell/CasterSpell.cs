using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	public class CasterSpell
	{
		public Spell spell { get; private set; }
		public bool isActive { get; private set; }
		public ISpellCaster owner { get; private set; }

		public CasterSpell(Spell spell, ISpellCaster owner)
		{
			this.spell = spell;
			this.owner = owner;
			this.isActive = this.spell is ActiveSpell;
		}

		public void SetSpell(Spell newSpell)
		{
			this.spell = newSpell;
			this.isActive = this.spell is ActiveSpell;
		}

		public CasterSpell CloneToOwner(ISpellCaster newOwner)
		{
			return new CasterSpell(spell, newOwner);
		}

	}
}
