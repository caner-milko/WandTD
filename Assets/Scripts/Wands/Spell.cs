using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    /// <summary>
    /// A spell has stats(will change), a unique static name <see cref="SpellName"/> and <see cref="addToGroup(SpellGroupBuilder)"/>, ticking and casting methods
    /// </summary>
    public abstract class Spell
    {
        public float castModifier, rechargeModifier;
        public int mana;

        /// <summary>
        /// Every spell class should have a static unique name<br/>
        /// Active Spell Naming Format: "AS_camelCase"<br/>
        /// Passive Spell Naming Format: "PS_camelCase"
        /// </summary>
        /// <returns></returns>
        public abstract string SpellName();

        /// <summary>
        /// Exists to be inherited
        /// </summary>
        /// <param name="group"></param>
        public virtual void addToGroup(SpellGroupBuilder group)
        {

        }
        /// <summary>
        /// Exists to be inherited
        /// </summary>
        /// <param name="casted"></param>
        public virtual void OnBeforeCast(CastedSpell casted) { }
        /// <summary>
        /// Exists to be inherited
        /// </summary>
        /// <param name="casted"></param>
        public virtual void OnCast(CastedSpell casted) { }
        /// <summary>
        /// Exists to be inherited
        /// </summary>
        /// <param name="casted"></param>
        public virtual void OnAfterCast(CastedSpell casted) { }
        /// <summary>
        /// Exists to be inherited
        /// </summary>
        /// <param name="casted"></param>
        public virtual void OnBeforeTick(CastedSpell casted) { }
        /// <summary>
        /// Exists to be inherited
        /// </summary>
        /// <param name="casted"></param>
        public virtual void OnTick(CastedSpell casted) { }
        /// <summary>
        /// Exists to be inherited
        /// </summary>
        /// <param name="casted"></param>
        public virtual void OnAfterTick(CastedSpell casted) { }
    }
}
