using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public abstract class Spell
    {
        public float castModifier, rechargeModifier;
        public int mana;

        public abstract string SpellType();

        public abstract void addToGroup(SpellGroupBuilder group);
    }
}
