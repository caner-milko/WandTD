using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public abstract class ActiveSpell : Spell
    {

        public int triggerCount;

        public CastedSpell prefab;

        public CastedSpell CreateCasted(ISpellCaster caster, ISpellTarget target, SingleSpellGroup group)
        {
            CastedSpell casted = GameObject.Instantiate<CastedSpell>(prefab);
            casted.target = target;
            casted.spellGroup = group;

            SpellManager.manager.AddCastedSpell(casted);
            return casted;
        }

        public virtual void OnBeforeCast(CastedSpell casted) { }

        public virtual void OnCast(CastedSpell casted) { }

        public virtual void OnAfterCast(CastedSpell casted) { }

        public virtual void OnBeforeTick(CastedSpell casted) { }

        public virtual void OnTick(CastedSpell casted) { }

        public virtual void OnAfterTick(CastedSpell casted) { }

        public abstract bool checkHit(CastedSpell casted, out List<ISpellTarget> hitList);

        public abstract SpellHit Hit(CastedSpell from, ISpellTarget target);

        public override void addToGroup(SpellGroupBuilder group)
        {
            group.AddChildSpellGroup(triggerCount);
        }

    }
}