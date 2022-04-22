using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
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
