using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wands;

namespace wtd.wands.spells
{
    public class MulticastTestPassive : MulticastPassiveSpell
    {
        public MulticastTestPassive(int multicastCount) : base(multicastCount)
        {

        }

        public override string SpellName()
        {
            return "PS_multicastTest";
        }
    }
}
