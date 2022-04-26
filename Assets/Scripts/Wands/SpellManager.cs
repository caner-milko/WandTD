using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wands.spells; //debug purpose

namespace wtd.wands
{
    /// <summary>
    /// Singleton manager for the spells<br/>
    /// Each <see cref="CastedSpell"/> is added here, updated by the manager<br/>
    /// Will probably change later
    /// </summary>
    public class SpellManager : MonoBehaviour
    {

        public static SpellManager instance;
        //For debug
        [SerializeField]
        CastedSpell FirePrefab;
        [SerializeField]
        CastedSpell BluePrefab;

        /// <summary>
        /// Each <see cref="Spell"/> implemented should be added to the manager, there is only a single instance of each spell
        /// </summary>
        List<Spell> spells = new List<Spell>();

        /// <summary>
        /// Currently alive <see cref="CastedSpell"/>s
        /// </summary>
        List<CastedSpell> castedSpells = new List<CastedSpell>();

        private void Awake()
        {
            instance = this;

            ///For Debug, later will be read from <see cref="ScriptableObject"/>s or files
            FireSpell fs = new FireSpell();
            fs.prefab = FirePrefab;
            spells.Add(fs);

            BlueSpell bs = new BlueSpell();
            bs.prefab = BluePrefab;
            spells.Add(bs);

            TestPassive tp = new TestPassive();
            spells.Add(tp);
            MulticastTestPassive mtp = new MulticastTestPassive(2);
            spells.Add(mtp);
        }

        private void Update()
        {
            //Update every living casted spell
            foreach (CastedSpell cs in castedSpells)
            {
                TickCastedSpell(cs);
            }
        }

        /// <summary>
        /// In order ticking for active and passive spells<br/>
        /// Order:<br/>
        /// Active's <see cref="Spell.OnBeforeTick(CastedSpell)"/> => Passives' <see cref="Spell.OnBeforeTick(CastedSpell)"/> => Active's <see cref="Spell.OnTick(CastedSpell)"/> => Active's <see cref="Spell.OnTick(CastedSpell)"/> => Active's <see cref="Spell.OnAfterTick(CastedSpell)"/> => Passives' <see cref="Spell.OnAfterTick(CastedSpell)"/> 
        /// </summary>
        /// <param name="casted">Casted spell to be ticked</param>
        private void TickCastedSpell(CastedSpell casted)
        {
            SingleSpellGroup group = casted.spellGroup;
            ActiveSpell spell = group.active;
            spell.OnBeforeTick(casted);
            foreach (PassiveSpell passive in group.passives)
                passive.OnBeforeTick(casted);
            spell.OnTick(casted);
            foreach (PassiveSpell passive in group.passives)
                passive.OnTick(casted);
            spell.OnAfterTick(casted);
            foreach (PassiveSpell pspell in group.passives)
            {
                pspell.OnAfterTick(casted);
            }
        }

        /// <summary>
        /// In order casting for active and passive spells<br/>
        /// Order:<br/>
        /// Active's <see cref="Spell.OnBeforeCast(CastedSpell)"/> => Passives' <see cref="Spell.OnBeforeCast(CastedSpell)"/> => Active's <see cref="Spell.OnCast(CastedSpell)"/> => Active's <see cref="Spell.OnCast(CastedSpell)"/> => Active's <see cref="Spell.OnAfterCast(CastedSpell)"/> => Passives' <see cref="Spell.OnAfterCast(CastedSpell)"/>
        /// </summary>
        /// <param name="casted"></param>
        public void AddCastedSpell(CastedSpell casted)
        {
            SingleSpellGroup group = casted.spellGroup;
            ActiveSpell spell = group.active;
            castedSpells.Add(casted);
            spell.OnBeforeCast(casted);
            foreach (PassiveSpell pspell in group.passives)
            {
                pspell.OnBeforeCast(casted);
            }
            spell.OnCast(casted);
            foreach (PassiveSpell pspell in group.passives)
            {
                pspell.OnCast(casted);
            }
            spell.OnAfterCast(casted);
            foreach (PassiveSpell pspell in group.passives)
            {
                pspell.OnAfterCast(casted);
            }
        }

        /// <summary>
        /// Get spell by name, each spell should have a single instance kept in the manager
        /// </summary>
        /// <param name="name">name of the spell</param>
        /// <returns></returns>
        public Spell GetSpellByType(string name)
        {
            foreach (Spell spell in spells)
            {
                if (spell.SpellName() == name)
                    return spell;
            }
            return null;
        }

    }
}
