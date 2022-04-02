using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wands.targets;

namespace wtd.wands
{
    public class Wand : MonoBehaviour, ISpellCaster, ISpellTarget
    {

        public int capacity;

        public int mana;

        public double castDelay, rechargeRate;

        public double curCastDelay, curRechargeRate;

        public int castCount;

        public ISpellCaster owner;

        public List<CasterSpell> spells = new List<CasterSpell>();

        public List<CasterSpell> remSpells = new List<CasterSpell>();

        public SpellGroupBuilder curBuilder;

        public bool Shoot(ISpellTarget target, out List<CastedSpell> casted)
        {
            SpellGroupBase group;
            if (PrepareSpellGroup(new List<PassiveSpell>(), out group))
            {
                casted = group.Cast(target);
                return true;
            }
            casted = null;
            return false;
        }

        public bool PrepareSpellGroup(List<PassiveSpell> passives, out SpellGroupBase group)
        {
            if (remSpells.Count == 0)
            {
                remSpells = new List<CasterSpell>(spells);
            }
            SpellGroupBuilder builder = new SpellGroupBuilder(this, castCount);

            curBuilder = builder;

            group = builder.Build();
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

        public CasterSpell NextSpell()
        {
            if (remSpells.Count == 0)
            {
                return null;
            }
            CasterSpell next = remSpells[0];
            remSpells.RemoveAt(0);
            return next;
        }

        public CasterSpell AddSpell(Spell spell)
        {
            CasterSpell casterSpell = new CasterSpell(spell, this, spells.Count);
            spells.Add(casterSpell);
            return casterSpell;
        }

    }
}
