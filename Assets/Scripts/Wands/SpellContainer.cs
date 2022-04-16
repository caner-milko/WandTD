using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    /// <summary>
    /// Holds maximum of given number of spells that fits given validators.
    /// </summary>
    public class SpellContainer : IEnumerable<CasterSpell>
    {
        // Max number of spells that this container can hold.
        private int _capacity;

        // List of spells this container holds.
        private List<SpellSlot> _spellSlots;

        // List of validators that every spell in this container has to satisfy.
        private List<Func<CasterSpell, bool>> _spellValidators;

        // Number of spells in this container.
        public int Count {
            get {
                int count = 0;
                foreach (CasterSpell spell in this) count++;
                return count;
            }
        }

        // Max number of spells that this container can hold.
        public int Capacity {
            get => _capacity;
            set {
                _spellSlots.Capacity = _capacity = value;
            }
        }

        /// <summary>
        /// Create a spell container with given capacity.
        /// </summary>
        /// <param name="capacity">Max number of spells that this container can hold.</param>
        public SpellContainer(int capacity) {
            Capacity = capacity;
            _spellSlots = new List<SpellSlot>(capacity);
        }

        /// <summary>
        /// Create a spell container with given capacity.
        /// </summary>
        /// <param name="capacity">Max number of spells that this container can hold.</param>
        /// <param name="spellValidators">List of spell validator functions that every spell in this container has to satisfy.</param>
        public SpellContainer(int capacity, IEnumerable<Func<CasterSpell, bool>> spellValidators) : this(capacity) {
            _spellValidators = new List<Func<CasterSpell, bool>>(spellValidators);
        }

        /// <summary>
        /// Adds given spell to this container.
        /// </summary>
        /// <param name="spell">Spell to be added to this container.</param>
        /// <returns>true if spell is added, false otherwise.</returns>
        public bool AddSpell(CasterSpell spell) {
            if(Count >= Capacity){
                return false;
            }
            else foreach(Func<CasterSpell, bool> spellValidator in _spellValidators) {
                if(!spellValidator.Invoke(spell)){
                    return false;
                }
            }
            _spellSlots.Add(new SpellSlot(spell));
            return true;
        }

        public bool InsertSpell(int index, CasterSpell spell) {
            if(Count >= Capacity || index >= Capacity){
                return false;
            }
            _spellSlots.Insert(index, new SpellSlot(spell));
            return true;
        }

        public bool RemoveSpell(CasterSpell spell) {
            foreach(SpellSlot spellSlot in _spellSlots) {
                if(spellSlot.Spell.Equals(spell)){
                    spellSlot.Spell = null;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveSpell(int index) {
            _spellSlots[index].Spell = null;
            return true;
        }

        /// <summary>
        /// Adds spell validator function to this container.
        /// </summary>
        /// <param name="spellValidator">Function that is to be added as spell validator to this container.</param>
        /// <param name="invalidSpells">Spells that were in this container before new validator was added in that do not satisfy it.</param>
        /// <returns>true if given function was added as spell validator, false otherwise.</returns>
        public bool AddSpellValidator(Func<CasterSpell, bool> spellValidator, out CasterSpell[] invalidSpells) {
            _spellValidators.Add(spellValidator);

            List<CasterSpell> _invalidSpells = new List<CasterSpell>();

            foreach (SpellSlot spellSlot in _spellSlots)
            {
                if(!spellValidator.Invoke(spellSlot.Spell)){
                    _invalidSpells.Add(spellSlot.Spell);
                    spellSlot.Spell = null;
                }   
            }

            invalidSpells = _invalidSpells.ToArray();

            return true;
        }

        public bool RemoveSpellValidator(Func<CasterSpell, bool> spellValidator) {
            return _spellValidators.Remove(spellValidator);
        }

        public IEnumerator<CasterSpell> GetEnumerator()
        {
            return new SpellContainerEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Enumerator for SpellContainer class that skips empty spells upon moving to next spell.
        /// </summary>
        private class SpellContainerEnumerator : IEnumerator<CasterSpell> {

            private SpellSlot[] _spellSlots;
            private int index = 0;

            object IEnumerator.Current => Current;
            public CasterSpell Current => _spellSlots[index].Spell;

            public SpellContainerEnumerator(SpellContainer spellContainer) {
                _spellSlots = spellContainer._spellSlots.GetRange(0, spellContainer.Capacity).ToArray();
            }

            public bool MoveNext() {
                while(_spellSlots[++index].Spell == null);
                return index < _spellSlots.Length;
            }

            public void Reset() {
                index = 0;
            }

            public void Dispose() {
                _spellSlots = null;
            }
        }
    }
}
