using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public abstract class MulticastPassiveSpell : PassiveSpell
    {
        public int multicastCount { get; private set; }

        public MulticastPassiveSpell(int multicastCount)
        {
            this.multicastCount = multicastCount;
        }

        public override void addToGroup(SpellGroupBuilder group)
        {
            group.increaseRemCastCount(multicastCount);
        }
    }
}
