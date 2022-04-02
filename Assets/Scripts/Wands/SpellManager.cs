using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wands.spells; //debug purpose

namespace wtd.wands
{
    public class SpellManager : MonoBehaviour
    {

        public static SpellManager manager;
        List<Spell> spells = new List<Spell>();
        List<CastedSpell> castedSpells = new List<CastedSpell>();
        [SerializeField]
        CastedSpell FirePrefab;
        [SerializeField]
        CastedSpell BluePrefab;


        private void Awake()
        {
            manager = this;
        }

        private void Start()
        {
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
            foreach (CastedSpell cs in castedSpells)
            {
                TickCastedSpell(cs);
            }
        }

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
        }

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
        }

        public Spell GetSpellByType(string type)
        {
            foreach (Spell spell in spells)
            {
                if (spell.SpellType() == type)
                    return spell;
            }
            return null;
        }

    }
}
