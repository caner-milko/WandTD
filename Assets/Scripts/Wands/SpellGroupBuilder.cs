using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace wtd.wands
{
    //A builder builds the spell group required for creating the active spell
    //It works recursively, adding new spell groups as childs if needed(mainly for trigger spells)
    //Every time a spell is added to the builder, a method is called in the spell
    //For example, multicast passive spells increase the remaining cast count
    public class SpellGroupBuilder
    {
        /// <summary>
        /// passives in the group
        /// </summary>
        public List<PassiveSpell> passives { get; private set; } = new List<PassiveSpell>();
        /// <summary>
        /// active spell is null if the group is a multicast group
        /// </summary>
        public ActiveSpell active { get; private set; }
        /// <summary>
        /// 2 cases:
        /// 1. Group is multicast, then childs are <see cref="SingleSpellGroup"/>s which will create the <see cref="MultiSpellGroup"/> at the end
        /// 2. Group is single cast, then childs contains only 1 element and it is the child group of the active spell
        /// </summary>
        public List<SpellGroupBase> childGroups { get; private set; } = new List<SpellGroupBase>();

        /// <summary>
        /// caster of the group
        /// </summary>
        public ISpellCaster caster { get; private set; }

        /// <summary>
        /// if group is multicast, create a MultiSpellGroup at the end, if not create a SingleSpellGroup
        /// MultiSpellGroup consists of multiple <see cref="SingleSpellGroup"> which share the same passives
        /// </summary>
        private bool multiCast;
        /// <summary>
        /// current remaining cast count
        /// </summary>
        private int remCastCount;

        /// <summary>
        /// A spell group is created with a caster and given castCount
        /// </summary>
        /// <param name="caster">Caster who shoot the spell, generally a wand</param>
        /// <param name="castCount">How many spells should be cast at once in the group</param>
        public SpellGroupBuilder(ISpellCaster caster, int castCount)
        {
            this.caster = caster;
            ///group is multicast if there are multiple spells cast
            ///may become a multicastspell later with the <see cref="increaseRemCastCount(int)"/> method
            this.multiCast = castCount > 1;
            this.remCastCount = castCount;
            StartBuild();
        }
        /// <summary>
        /// Creates a spell group from the given CasterSpell
        /// Must be a single cast group at the end
        /// </summary>
        /// <param name="spell">active spell of the group</param>
        private SpellGroupBuilder(CasterSpell spell)
        {
            this.caster = spell.owner;
            this.multiCast = false;
            this.remCastCount = 0;
            this.active = (ActiveSpell)spell.spell;
            //might be a trigger spell
            spell.spell.addToGroup(this);
            StartBuild();
        }

        private void StartBuild()
        {
            /// add spells until 0 remaining casts or there are no remaining spells in the caster 
            // TODO: reached mana limit
            while (remCastCount > 0)
            {
                CasterSpell selected = caster.NextSpell();
                //no remaining spells
                if (selected == null)
                    break;

                //TODO: manacheck

                //if the selected spell is active, add as a active spell in the childGroups or as the active spell
                if (selected.isActive)
                {
                    if (multiCast)
                    {
                        childGroups.Add(new SpellGroupBuilder(selected).Build());
                        remCastCount--;
                    }
                    else
                    {
                        this.active = (ActiveSpell)selected.spell;
                        selected.spell.addToGroup(this);
                        break;
                    }
                }
                //Else, add as a passive spell
                else
                {
                    passives.Add((PassiveSpell)selected.spell);
                    selected.spell.addToGroup(this);
                }
            }
        }
        /// <summary>
        /// Add castCount amount of spells as child group
        /// Only used for single casts
        /// Mainly used for trigger spells <see cref="ActiveSpell.addToGroup(SpellGroupBuilder)"/> 
        ///</summary>
        /// <param name="castCount">CastCount of the group to be added as child, can be 0 which will do nothing</param>

        public void AddChildSpellGroup(int castCount)
        {
            if (castCount > 0)
                childGroups.Add(new SpellGroupBuilder(caster, castCount).Build());
        }

        /// <summary>
        /// Build the resulting <see cref="SpellGroupBase"/>
        /// </summary>
        /// <returns> Can be a <see cref="MultiSpellGroup"/> or <see cref="SingleSpellGroup"/> based on cast count</returns>
        public SpellGroupBase Build()
        {
            /// if multicast, then all child groups are <see cref="SingleSpellGroup"/>s
            /// Convert the list
            /// configure the child groups
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
                ///if has a child group, configure the child group then return
                if (childGroups.Count > 0)
                {
                    SingleSpellGroup spg = new SingleSpellGroup(caster, passives, active, childGroups[0]);
                    spg.childGroup.SetParent(spg);
                    return spg;
                }
                else
                {
                    return new SingleSpellGroup(caster, passives, active);
                }

            }
        }

        /// <summary>
        /// increase the remaining cast count
        /// if the <paramref name="amount"/> is bigger than 0, then change the group to multicast
        /// </summary>
        /// <param name="amount">amount to increase <see cref="remCastCount"/></param>
        public void increaseRemCastCount(int amount)
        {
            remCastCount += amount;
            multiCast = multiCast || (amount > 0);
        }

    }
}