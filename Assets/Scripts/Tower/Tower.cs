using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wands;

namespace wtd.tower
{
    public class Tower : MonoBehaviour, ISpellCaster, ISpellTarget
    {
        public string CasterType()
        {
            throw new System.NotImplementedException();
        }

        public Vector3 GetPosition()
        {
            throw new System.NotImplementedException();
        }

        public string GetTargetType()
        {
            throw new System.NotImplementedException();
        }

        public CasterSpell NextSpell()
        {
            throw new System.NotImplementedException();
        }
    }
}