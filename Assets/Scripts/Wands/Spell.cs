using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public abstract class Spell
    {
        public double CastModifier, RechargeModifier;
        public int Mana;

        public abstract string SpellType();
    }
}
