using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public class CastedSpell : MonoBehaviour
    {
        public ActiveSpell spell { get; set; }
        public SpellGroup spellGroup { get; set; }

        public SpellTarget target { get; set; }
        public SpellCaster caster { get; set; }
        public Vector3 curPosition, castPosition;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}