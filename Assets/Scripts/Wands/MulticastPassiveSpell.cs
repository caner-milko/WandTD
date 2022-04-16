using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    /// <summary>
    /// Spells that increase cast count of the group, resulting in multicast groups
    /// </summary>
    public abstract class MulticastPassiveSpell : PassiveSpell
    {
        /// <summary>
        /// Cast count to increase
        /// </summary>
        public int multicastCount { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="multicastCount">Cast count to increase</param>
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
