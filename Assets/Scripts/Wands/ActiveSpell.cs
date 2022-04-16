using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    /// <summary>
    /// Every Active Spell should be inherited from this class <br/>
    /// An active spell has a <see cref="CastedSpell"/> prefab which will be instantiated when cast. <br/>
    /// Can be a trigger spell, <see cref="triggerCount"/>
    /// </summary>
    public abstract class ActiveSpell : Spell
    {

        /// <summary>
        /// Amount of spells should be cast when spell hits
        /// </summary>
        public int triggerCount;

        /// <summary>
        /// Prefab to be instantiated at <see cref="CreateCasted(ISpellCaster, ISpellTarget, SingleSpellGroup)"/>
        /// </summary>
        public CastedSpell prefab;

        /// <summary>
        /// Creates a <see cref="GameObject"/> from the given prefab, adds the group 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        public CastedSpell CreateCasted(ISpellTarget target, SingleSpellGroup group)
        {
            CastedSpell casted = GameObject.Instantiate<CastedSpell>(prefab);
            casted.target = target;
            casted.spellGroup = group;

            SpellManager.instance.AddCastedSpell(casted);
            return casted;
        }

        /// <summary>
        /// TODO
        /// Check hit of the spell each tick, if hit return the targets hit by the spell as <see cref="SpellHit"/>s, which will be used in <see cref="Hit(CastedSpell, List{SpellHit})/>
        /// </summary>
        /// <param name="casted"></param>
        /// <param name="hitList"><see cref="ISpellTarget"/>s hit by the spell and other information</param>
        /// <returns></returns>
        //public abstract bool checkHit(CastedSpell casted, out List<SpellHit> hitList);

        /// <summary>
        /// Calls <see cref="Hit"/> for each target, then <see cref="DoHit(CastedSpell, List{SpellHit})"/>, then deletes the objecet
        /// </summary>
        /// <param name="from"></param>
        /// <param name="hitList"></param>
        public void Hit(CastedSpell from, List<SpellHit> hitList)
        {
            //hitList.ForEach(hit => HitTarget(from, hit));
            DoHit(from, hitList);
            GameObject.Destroy(from.gameObject);
        }

        /// <summary>
        /// Exists to be overriden, not needed
        /// </summary>
        /// <param name="from"></param>
        /// <param name="hitList"></param>
        public void DoHit(CastedSpell from, List<SpellHit> hitList) { }

        /// <summary>
        /// TODO, damage calc etc.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        //public abstract SpellHit HitTarget(CastedSpell from, SpellHit target);

        public override void addToGroup(SpellGroupBuilder group)
        {
            group.AddChildSpellGroup(triggerCount);
        }

    }
}