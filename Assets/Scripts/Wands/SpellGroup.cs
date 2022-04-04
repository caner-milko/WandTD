using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace wtd.wands
{
    public class SpellGroupBuilder
    {
        public List<PassiveSpell> passives { get; private set; } = new List<PassiveSpell>();

        public ActiveSpell active { get; private set; }

        public List<SpellGroupBase> childGroups { get; private set; } = new List<SpellGroupBase>();

        public ISpellCaster caster { get; private set; }

        bool multiCast;
        int remCastCount;

        public SpellGroupBuilder(ISpellCaster caster, int castCount)
        {
            this.caster = caster;
            this.multiCast = castCount > 1;
            this.remCastCount = castCount;
            StartBuild();
        }
        public SpellGroupBuilder(CasterSpell spell)
        {
            this.caster = spell.owner;
            this.multiCast = false;
            this.remCastCount = 0;
            this.active = (ActiveSpell)spell.spell;
            spell.spell.addToGroup(this);
            StartBuild();
        }

        public void StartBuild()
        {
            while (remCastCount > 0)
            {
                CasterSpell selected = caster.NextSpell();
                if (selected == null)
                    break;
                //TODO manacheck
                if (selected.isActive)
                {
                    if (multiCast)
                    {
                        childGroups.Add(new SpellGroupBuilder(selected).Build());
                    }
                    else
                    {
                        this.active = (ActiveSpell)selected.spell;
                        selected.spell.addToGroup(this);
                        break;
                    }
                    remCastCount--;
                }
                else
                {
                    passives.Add((PassiveSpell)selected.spell);
                    selected.spell.addToGroup(this);
                }
            }
        }

        public void AddChildSpellGroup(int castCount)
        {
            if (castCount > 0)
                childGroups.Add(new SpellGroupBuilder(caster, castCount).Build());
        }

        public SpellGroupBase Build()
        {
            if (multiCast)
            {
                List<SingleSpellGroup> spellGroups = childGroups.Cast<SingleSpellGroup>().ToList();
                MultiSpellGroup group = new MultiSpellGroup(caster, passives, spellGroups);
                foreach (SingleSpellGroup singleSpellGroup in spellGroups)
                {
                    singleSpellGroup.passives.AddRange(passives);
                    singleSpellGroup.SetParent(group);
                }
                return group;
            }
            else
            {
                return new SingleSpellGroup(caster, passives, active);
            }
        }

        public void increaseRemCastCount(int amount)
        {
            remCastCount += amount;
            multiCast = multiCast || (amount > 0);
        }

    }

    public abstract class SpellGroupBase
    {
        public List<PassiveSpell> passives { get; private set; } = new List<PassiveSpell>();

        public SpellGroupBase parent { get; private set; }

        public bool hasParent
        {
            get
            {
                return parent != null;
            }
        }

        public ISpellCaster caster { get; private set; }

        protected SpellGroupBase(ISpellCaster caster, List<PassiveSpell> passives)
        {
            this.caster = caster;
            this.passives = passives;
        }

        public abstract float GetCastDelay();

        public abstract float GetRechargeDelay();

        public abstract List<CastedSpell> Cast(ISpellTarget target);

        public void SetParent(SpellGroupBase parent)
        {
            if (this.parent != null)
            {
                throw new NotSupportedException("Trying to change the parent of a already parented group.");
            }
            this.parent = parent;
        }

    }

    public class SingleSpellGroup : SpellGroupBase
    {
        public ActiveSpell active { get; private set; }

        public SpellGroupBase childGroup { get; private set; }

        internal SingleSpellGroup(ISpellCaster caster, List<PassiveSpell> passives, ActiveSpell active) : base(caster, passives)
        {
            this.active = active;
        }

        public override float GetCastDelay()
        {
            float delay = 0;
            foreach (PassiveSpell spell in passives)
            {
                delay += spell.castModifier;
            }
            delay += GetCastDelayWOPassives();
            return delay;
        }

        public override float GetRechargeDelay()
        {
            float delay = 0;
            foreach (PassiveSpell spell in passives)
            {
                delay += spell.castModifier;
            }
            delay += GetRechargeDelayWOPassives();
            return delay;
        }

        public float GetCastDelayWOPassives()
        {
            return (childGroup != null ? childGroup.GetCastDelay() : 0.0f) + active.castModifier;
        }

        public float GetRechargeDelayWOPassives()
        {
            return (childGroup != null ? childGroup.GetRechargeDelay() : 0.0f) + active.rechargeModifier;
        }

        public override List<CastedSpell> Cast(ISpellTarget target)
        {
            List<CastedSpell> castedSpells = new List<CastedSpell>();
            castedSpells.Add(CastSingle(target));
            return castedSpells;
        }

        public CastedSpell CastSingle(ISpellTarget target)
        {
            return active.CreateCasted(caster, target, this);
        }

    }

    public class MultiSpellGroup : SpellGroupBase
    {

        public List<SingleSpellGroup> spells { get; private set; } = new List<SingleSpellGroup>();

        internal MultiSpellGroup(ISpellCaster caster, List<PassiveSpell> passives, List<SingleSpellGroup> spells) : base(caster, passives)
        {
            this.spells = spells;
        }

        public override float GetCastDelay()
        {
            float delay = 0;
            foreach (PassiveSpell spell in passives)
            {
                delay += spell.castModifier;
            }
            foreach (SingleSpellGroup group in spells)
            {
                delay += group.GetCastDelayWOPassives();
            }
            return delay;
        }

        public override float GetRechargeDelay()
        {
            float delay = 0;
            foreach (PassiveSpell spell in passives)
            {
                delay += spell.castModifier;
            }
            foreach (SingleSpellGroup group in spells)
            {
                delay += group.GetRechargeDelayWOPassives();
            }
            return delay;
        }

        public override List<CastedSpell> Cast(ISpellTarget target)
        {
            List<CastedSpell> castedSpells = new List<CastedSpell>();
            foreach (SingleSpellGroup spg in spells)
                castedSpells.Add(spg.CastSingle(target));
            return castedSpells;
        }


    }
}
