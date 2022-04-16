using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    /// <summary>
    /// Every spell <see cref="GameObject"/> should be a <see cref="CastedSpell"/><br/>
    /// Every casted spell contains a <see cref="SingleSpellGroup"/> in it, its target 
    /// </summary>
    public class CastedSpell : MonoBehaviour
    {
        public SingleSpellGroup spellGroup { get; set; }

        public ISpellTarget target { get; set; }

        public Vector3 curPosition, castPosition;
    }
}