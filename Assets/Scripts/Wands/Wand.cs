using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wands.targets;

namespace wtd.wands
{
    public class Wand : MonoBehaviour, SpellCaster, SpellTarget
    {

        public int capacity;

        public int mana;

        public double castDelay, rechargeRate;

        public double curCastDelay, curRechargeRate;

        public SpellCaster owner;

        public List<Spell> spells = new List<Spell>();

        public List<Spell> remSpells = new List<Spell>();

        public List<Spell> lastCast = new List<Spell>(), activeCast = new List<Spell>();

        public SpellGroup lastCastGroup, activeCastGroup;

        public bool Shoot(SpellTarget target, out CastedSpell casted)
        {
            SpellGroup group;
            Cast(target, new List<PassiveSpell>(), out group);
            casted = group.Cast();
            return true;
        }

        public bool Cast(SpellTarget target, List<PassiveSpell> passives, out SpellGroup group)
        {
            if (remSpells.Count == 0)
            {
                remSpells = new List<Spell>(spells);
            }
            group = NextGroup(target);
            group.Passives.AddRange(passives);
            lastCastGroup = activeCastGroup;
            lastCast = activeCast;
            activeCastGroup = group;
            return true;
        }

        public string CasterType()
        {
            return "wand";
        }

        public string GetTargetType()
        {
            return "wand";
        }

        public Vector3 GetPosition()
        {
            return owner.GetPosition();
        }

        public SpellGroup NextGroup(SpellTarget target)
        {
            SpellGroup group = new SpellGroup(this, target);
            Spell selected;
            while (true)
            {
                selected = nextSpell();
                if (selected == null)
                {
                    break;
                }
                remSpells.Remove(selected);
                activeCast.Add(selected);
                if (selected is ActiveSpell)
                {
                    group.Active = (ActiveSpell)selected;
                    break;
                }
                group.Passives.Add((PassiveSpell)selected);
            }
            return group;
        }

        public Spell nextSpell()
        {
            if (remSpells.Count == 0)
            {
                return null;
            }
            return remSpells[0];
        }

    }
}
