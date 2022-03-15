using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public interface SpellTarget
    {
        public Vector3 GetPosition();

        public string GetTargetType();
    }
}
