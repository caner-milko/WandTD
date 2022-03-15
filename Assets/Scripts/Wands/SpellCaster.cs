using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public interface SpellCaster
    {
        public string CasterType();

        public Vector3 GetPosition();
    }
}