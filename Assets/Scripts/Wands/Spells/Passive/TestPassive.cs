using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wands;
namespace wtd.wands.spells
{
    public class TestPassive : PassiveSpell
    {
        public override string SpellType()
        {
            return "PS_test";
        }

        public override void OnTick(CastedSpell casted)
        {
            Vector3 dif = casted.target.GetPosition() - casted.transform.position;
            casted.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (dif.magnitude + 0.05f);
        }

    }
}
