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
        private List<CasterSpell> _spells;

        // List of validators that every spell in this container has to satisfy.
        private List<Func<CasterSpell, bool>> _spellValidators;

        /// <summary>
        /// Create a spell container with given capacity.
        /// </summary>
        /// <param name="capacity">Max number of spells that this container can hold.</param>
        public SpellContainer(int capacity) {
            _capacity = capacity;
            _spells = new List<CasterSpell>(capacity);
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
            if(_spells.Count >= _capacity){
                return false;
            }
            else foreach(Func<CasterSpell, bool> spellValidator in _spellValidators) {
                if(!spellValidator.Invoke(spell)){
                    return false;
                }
            }
            _spells.Add(spell);
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

            foreach (CasterSpell spell in this)
            {
                if(!spellValidator.Invoke(spell)){
                    _invalidSpells.Add(spell);
                    _spells.Remove(spell);
                }
            }

            invalidSpells = _invalidSpells.ToArray();

            return true;
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

            private CasterSpell[] _spells;
            private int index = 0;

            object IEnumerator.Current => Current;
            public CasterSpell Current => _spells[index];

            public SpellContainerEnumerator(SpellContainer spellContainer) {
                _spells = spellContainer._spells.ToArray();
            }

            public bool MoveNext() {
                while(_spells[++index] == null);
                return index < _spells.Length;
            }

            public void Reset() {
                index = 0;
            }

            public void Dispose() {
                _spells = null;
            }
        }
    }
}
