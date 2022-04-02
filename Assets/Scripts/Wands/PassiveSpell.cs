using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public abstract class PassiveSpell : Spell
    {
        public virtual void OnBeforeCast(CastedSpell casted) { }

        public virtual void OnCast(CastedSpell casted) { }

        public virtual void OnBeforeTick(CastedSpell casted) { }

        public virtual void OnTick(CastedSpell casted) { }

        public override void addToGroup(SpellGroupBuilder group)
        {

        }
    }
}
