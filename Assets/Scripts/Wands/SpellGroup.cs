using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public class SpellGroup
    {

        public List<PassiveSpell> Passives { get; private set; } = new List<PassiveSpell>();

        public ActiveSpell Active { get; set; }

        public List<SpellGroup> NextGroups { get; private set; } = new List<SpellGroup>();

        public SpellCaster Caster { get; private set; }

        public SpellTarget Target { get; private set; }

        public SpellGroup(SpellCaster caster, SpellTarget target)
        {
            this.Caster = caster;
            this.Target = target;
        }

        public double GetCastDelay()
        {
            double delay = 0;
            foreach (PassiveSpell spell in Passives)
            {
                delay += spell.CastModifier;
            }
            if (Active == null)
            {
                foreach (SpellGroup group in NextGroups)
                {
                    delay += group.GetCastDelay();
                }
            }
            else
            {
                delay += Active.CastModifier;
            }
            return delay;
        }

        public double GetRechargeDelay()
        {
            double delay = 0;
            foreach (PassiveSpell spell in Passives)
            {
                delay += spell.CastModifier;
            }
            if (Active != null)
            {
                delay += Active.CastModifier;
            }
            foreach (SpellGroup group in NextGroups)
            {
                delay += group.GetRechargeDelay();
            }
            return delay;
        }

        public CastedSpell Cast()
        {
            return Active.CreateCasted(Caster, Target, this);
        }
    }
}
