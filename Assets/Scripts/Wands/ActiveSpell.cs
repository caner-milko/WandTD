using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public abstract class ActiveSpell : Spell
    {
        public CastedSpell prefab;

        public CastedSpell CreateCasted(SpellCaster caster, SpellTarget target, SpellGroup group)
        {
            CastedSpell casted = GameObject.Instantiate<CastedSpell>(prefab);
            casted.spell = this;
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

        public abstract bool checkHit(CastedSpell casted, out List<SpellTarget> hitList);

        public abstract SpellHit Hit(CastedSpell from, SpellTarget target);
    }
}