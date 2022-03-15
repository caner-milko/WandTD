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


        private void Awake()
        {
            manager = this;
        }

        private void Start()
        {
            FireSpell fs = new FireSpell();
            fs.prefab = FirePrefab;
            spells.Add(fs);
            TestPassive tp = new TestPassive();
            spells.Add(tp);
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
            SpellGroup group = casted.spellGroup;
            ActiveSpell spell = casted.spell;
            spell.OnBeforeTick(casted);
            foreach (PassiveSpell passive in group.Passives)
                passive.OnBeforeTick(casted);
            spell.OnTick(casted);
            foreach (PassiveSpell passive in group.Passives)
                passive.OnTick(casted);
            spell.OnAfterTick(casted);
        }

        public void AddCastedSpell(CastedSpell casted)
        {
            SpellGroup group = casted.spellGroup;
            ActiveSpell spell = casted.spell;
            castedSpells.Add(casted);
            spell.OnBeforeCast(casted);
            foreach (PassiveSpell pspell in group.Passives)
            {
                pspell.OnBeforeCast(casted);
            }
            spell.OnCast(casted);
            foreach (PassiveSpell pspell in group.Passives)
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
