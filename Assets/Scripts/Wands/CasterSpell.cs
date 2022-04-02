using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public class CasterSpell : IComparable<CasterSpell>
    {
        public Spell spell { get; private set; }
        public bool isActive { get; private set; }
        public ISpellCaster owner { get; private set; }

        int slot;

        public CasterSpell(Spell spell, ISpellCaster owner, int slot)
        {
            this.spell = spell;
            this.owner = owner;
            this.slot = slot;
            this.isActive = spell is ActiveSpell;
        }

        public int CompareTo(CasterSpell other)
        {
            return this.slot.CompareTo(other.slot);
        }
    }
}
