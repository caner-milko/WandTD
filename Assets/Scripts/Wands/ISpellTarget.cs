using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands
{
    public interface ISpellTarget
    {
        public Vector3 GetPosition();

        public string GetTargetType();
    }
}
