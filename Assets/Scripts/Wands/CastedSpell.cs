using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public class CastedSpell : MonoBehaviour
    {
        public SingleSpellGroup spellGroup { get; set; }

        public ISpellTarget target { get; set; }
        public ISpellCaster caster { get; set; }
        public Vector3 curPosition, castPosition;
    }
}